using System.Text.Json;
using GylleneDroppen.Api.Dtos;
using GylleneDroppen.Api.Models;
using GylleneDroppen.Api.RedisModels;
using GylleneDroppen.Api.Repositories.Interfaces;
using GylleneDroppen.Api.Services.Interfaces;
using GylleneDroppen.Api.Utilities;
using GylleneDroppen.Api.Utilities.Interfaces;

namespace GylleneDroppen.Api.Services;

public class AuthService(IUserService userService, IUserRepository userRepository, IArgon2Hasher argon2Hasher, IJwtService jwtService, IRedisRepository redisRepository, IEmailService emailService) : IAuthService
{
    public async Task<ServiceResponse<LoginResponse>> LoginAsync(LoginRequest request)
    {
        var user = await userRepository.GetByEmailAsync(request.Email);

        if (user is null || !argon2Hasher.VerifyPassword(request.Password, user.PasswordHash, user.PasswordSalt))
        {
            return ServiceResponse<LoginResponse>.Failure("Invalid email or password.", 401);
        }

        var accessToken = jwtService.GenerateToken(user.Id);
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

        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = request.Email,
            PasswordHash = argon2Hasher.HashPassword(request.Password).Hash,
            PasswordSalt = argon2Hasher.HashPassword(request.Password).Salt
        };

        var confirmationCode = CodeGenerator.GenerateConfirmationCode(6);

        var pendingUser = new PendingUser
        {
            User = user,
            ConfirmationCode = confirmationCode
        };
        
        var pendingUserJson = JsonSerializer.Serialize(pendingUser);
        
        await redisRepository.SaveAsync($"pending_user:{user.Email}", pendingUserJson, TimeSpan.FromMinutes(15));
        
        return await emailService.SendEmailVerificationCodeAsync(user.Email, confirmationCode);
    }

    public async Task<ServiceResponse<MessageResponse>> LogoutAsync(string token, LogoutRequest request)
    {
        if (string.IsNullOrWhiteSpace(token)) return ServiceResponse<MessageResponse>.Failure("Invalid token.", 400);
        
        var storedRefreshToken = await jwtService.GetRefreshTokenAsync(request.UserId);
        
        if(storedRefreshToken is null || storedRefreshToken != request.RefreshToken)
            return ServiceResponse<MessageResponse>.Failure("Invalid refresh token.", 400);
        
        await jwtService.BlacklistTokenAsync(token);
        await jwtService.RevokeRefreshTokenAsync(request.UserId);
        
        return ServiceResponse<MessageResponse>.Success(new MessageResponse("Logged out successfully."));
    }

    public async Task<ServiceResponse<RefreshTokenResponse>> RefreshTokenAsync(RefreshTokenRequest request)
    {
        var storedRefreshToken = await jwtService.GetRefreshTokenAsync(request.UserId);
        if(storedRefreshToken == null || storedRefreshToken != request.RefreshToken)
            return ServiceResponse<RefreshTokenResponse>.Failure("Invalid refresh token.", 400);

        var newAccessToken = jwtService.GenerateToken(request.UserId);
        var newRefreshToken = jwtService.GenerateRefreshToken(request.UserId);
        
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
        
        if (pendingUser == null || pendingUser.ConfirmationCode != request.ConfirmationCode)
            return ServiceResponse<MessageResponse>.Failure("Invalid confirmation code.", 400);
        
        await userRepository.AddAsync(pendingUser.User);
        
        await redisRepository.DeleteAsync($"pending_user:{request.Email}");
        
        return ServiceResponse<MessageResponse>.Success(new MessageResponse("Email successfully verified. You can now log in."));
    }
}
