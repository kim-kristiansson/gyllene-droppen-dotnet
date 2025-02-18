using System.Text.Json;
using GylleneDroppen.Api.Dtos;
using GylleneDroppen.Api.Enums;
using GylleneDroppen.Api.Models;
using GylleneDroppen.Api.RedisModels;
using GylleneDroppen.Api.Repositories.Interfaces;
using GylleneDroppen.Api.Services.Interfaces;
using GylleneDroppen.Api.Utilities;
using GylleneDroppen.Api.Utilities.Interfaces;

namespace GylleneDroppen.Api.Services;

public class AuthService(IUserRepository userRepository, IArgon2Hasher argon2Hasher, IJwtService jwtService, IRedisRepository redisRepository, IEmailService emailService) : IAuthService
{
    public async Task<ServiceResponse<LoginResponse>> LoginAsync(LoginRequest request)
    {
        var user = await userRepository.GetByEmailAsync(request.Email);

        if (user is null || !argon2Hasher.VerifyPassword(request.Password, user.PasswordHash, user.PasswordSalt))
        {
            return ServiceResponse<LoginResponse>.Failure("Invalid email or password.", 401);
        }

        var accessToken = jwtService.GenerateToken(user);
        var refreshToken = jwtService.GenerateRefreshToken(user.Id);
        
        await jwtService.SaveRefreshTokenAsync(user.Id, refreshToken);

        return ServiceResponse<LoginResponse>.Success(new LoginResponse
        {
            Id = user.Id,
            Email = user.Email,
            AccessToken = accessToken,
            RefreshToken = refreshToken
        });
    }

    public async Task<ServiceResponse<MessageResponse>> RegisterAsync(RegisterRequest request)
    {
        if(await userRepository.GetByEmailAsync(request.Email) is not null)
            return ServiceResponse<MessageResponse>.Failure("Email already exists.", 400);
        
        var existingPendingUser = await redisRepository.GetAsync($"pending_user:{request.Email}");
        if (existingPendingUser != null)
            return ServiceResponse<MessageResponse>.Failure("A verification code has already been sent. Please check your email.", 400);

        var (hash, salt) = argon2Hasher.HashPassword(request.Password);
        
        var user = new User
        {
            Id = Guid.NewGuid(),
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            PasswordHash = hash,
            PasswordSalt = salt,
            Role = RoleType.User,
        };

        var confirmationCode = CodeGenerator.GenerateConfirmationCode(6);

        var pendingUser = new PendingUser
        {
            User = user,
            ConfirmationCode = confirmationCode
        };
        
        var pendingUserJson = JsonSerializer.Serialize(pendingUser);
        
        await redisRepository.SaveAsync($"pending_user:{user.Email}", pendingUserJson, TimeSpan.FromMinutes(15));
        
        return await emailService.SendEmailConfirmationCodeAsync(user.Email, confirmationCode);
    }

    public async Task<ServiceResponse<MessageResponse>> LogoutAsync(LogoutRequest request, string accessToken)
    {
        if (string.IsNullOrWhiteSpace(accessToken)) return ServiceResponse<MessageResponse>.Failure("Invalid token.", 400);
        
        var storedRefreshToken = await jwtService.GetRefreshTokenAsync(request.UserId);
        
        if(storedRefreshToken is null || storedRefreshToken != request.RefreshToken)
            return ServiceResponse<MessageResponse>.Failure("Invalid refresh token.", 400);
        
        await jwtService.RevokeTokensAsync(request.UserId, accessToken);
        
        return ServiceResponse<MessageResponse>.Success(new MessageResponse("Logged out successfully."));
    }

    public async Task<ServiceResponse<RefreshTokenResponse>> RefreshTokenAsync(RefreshTokenRequest request, string accessToken)
    {
        var storedRefreshToken = await jwtService.GetRefreshTokenAsync(request.UserId);
        if(storedRefreshToken == null || storedRefreshToken != request.RefreshToken)
            return ServiceResponse<RefreshTokenResponse>.Failure("Invalid refresh token.", 400);
        
        var user = await userRepository.GetByIdAsync(request.UserId);
        if(user == null)
            return ServiceResponse<RefreshTokenResponse>.Failure("Invalid refresh token.", 404);

        var newAccessToken = jwtService.GenerateToken(user);
        var newRefreshToken = jwtService.GenerateRefreshToken(request.UserId);
        
        await jwtService.RevokeTokensAsync(request.UserId, accessToken);
        
        await jwtService.SaveRefreshTokenAsync(request.UserId, newRefreshToken);
        
        var response = new RefreshTokenResponse()
        {
            AccessToken = newAccessToken, 
            RefreshToken = newRefreshToken
        };
        
        return ServiceResponse<RefreshTokenResponse>.Success(response);
    }

    public async Task<ServiceResponse<MessageResponse>> ConfirmEmailAsync(ConfirmEmailRequest request)
    {
        var pendingUserJson = await redisRepository.GetAsync($"pending_user:{request.Email}");
        
        if(pendingUserJson is null)
            return ServiceResponse<MessageResponse>.Failure("Confirmation code expired or invalid.", 400);
        
        var pendingUser = JsonSerializer.Deserialize<PendingUser>(pendingUserJson);
        
        if (pendingUser is null || !pendingUser.ConfirmationCode.Equals(request.ConfirmationCode, StringComparison.OrdinalIgnoreCase))
            return ServiceResponse<MessageResponse>.Failure("Invalid confirmation code.", 400);
        
        await userRepository.AddAsync(pendingUser.User);

        await userRepository.SaveChangesAsync();
        
        await redisRepository.DeleteAsync($"pending_user:{request.Email}");
        
        return ServiceResponse<MessageResponse>.Success(new MessageResponse("Email successfully verified. You can now log in."));
    }

    public async Task<ServiceResponse<MessageResponse>> RequestPasswordResetAsync(PasswordResetRequest request)
    {
        var user = await userRepository.GetByEmailAsync(request.Email);
        if(user is null)
            return ServiceResponse<MessageResponse>.Failure("Email not found", 400);

        var resetToken = CodeGenerator.GenerateConfirmationCode(6);
        var tokenEntry = new PasswordResetToken
        {
            Email = request.Email,
            Token = resetToken,
        };
        
        var serializedToken = JsonSerializer.Serialize(tokenEntry);
        await redisRepository.SaveAsync($"password_reset:{user.Email}", serializedToken, TimeSpan.FromMinutes(15));
        
        return await emailService.SendPasswordResetEmailAsync(user.Email, resetToken);
    }

    public async Task<ServiceResponse<MessageResponse>> ResetPasswordAsync(ResetPasswordRequest request)
    {
        var tokenJson = await redisRepository.GetAsync($"password_reset:{request.Email}");
        if(tokenJson is null)
            return ServiceResponse<MessageResponse>.Failure("Invalid token.", 400);
        
        var tokenEntry = JsonSerializer.Deserialize<PasswordResetToken>(tokenJson);
        if(tokenEntry is null || tokenEntry.Token != request.Token)
            return ServiceResponse<MessageResponse>.Failure("Invalid token.", 400);
        
        var (hash, salt) = argon2Hasher.HashPassword(request.Email);
        
        var user = await userRepository.GetByEmailAsync(request.Email);
        if(user == null)
            return ServiceResponse<MessageResponse>.Failure("Email not found", 400);
        
        user.PasswordHash = hash;
        user.PasswordSalt = salt;
        
        userRepository.Update(user);
        
        await redisRepository.DeleteAsync($"password_reset:{user.Email}");
        
        return ServiceResponse<MessageResponse>.Success(new MessageResponse("Password reset successfully."));
    }
}
