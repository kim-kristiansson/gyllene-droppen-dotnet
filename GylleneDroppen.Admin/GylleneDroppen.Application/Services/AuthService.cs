using System.Text.Json;
using GylleneDroppen.Application.Common.Results;
using GylleneDroppen.Application.Dtos.Common;
using GylleneDroppen.Application.Dtos.Email;
using GylleneDroppen.Application.Dtos.Shared.Auth;
using GylleneDroppen.Application.Interfaces.Shared.Repositories;
using GylleneDroppen.Application.Interfaces.Shared.Security;
using GylleneDroppen.Application.Interfaces.Shared.Services;
using GylleneDroppen.Application.Interfaces.Shared.Utilities;
using GylleneDroppen.Application.Utilities;
using GylleneDroppen.Domain.Entities;
using GylleneDroppen.Domain.Enums;
using GylleneDroppen.Domain.Models;

namespace GylleneDroppen.Application.Services.Shared;

public class AuthService(
    IUserRepository userRepository,
    IPasswordHasher argon2Hasher,
    IJwtService jwtService,
    IRedisRepository redisRepository,
    IEmailService emailService,
    ICookieManager cookieManager) : IAuthService
{
    public async Task<Result<MessageResponse>> LogoutAsync()
    {
        var accessToken = cookieManager.GetAccessToken();

        if (!string.IsNullOrEmpty(accessToken))
        {
            var userId = jwtService.GetUserIdFromToken(accessToken);

            if (userId != Guid.Empty)
            {
                var storedRefreshToken = await jwtService.GetRefreshTokenAsync(userId);

                if (!string.IsNullOrEmpty(storedRefreshToken)) await jwtService.RevokeTokensAsync(userId, accessToken);
            }
        }

        cookieManager.RemoveAuthCookies();

        return Result<MessageResponse>.Success(new MessageResponse("Logout successful"));
    }

    public async Task<Result<MessageResponse>> RefreshTokenAsync()
    {
        var accessToken = cookieManager.GetAccessToken();
        var refreshToken = cookieManager.GetRefreshToken();

        if (string.IsNullOrWhiteSpace(accessToken) || string.IsNullOrWhiteSpace(refreshToken))
            return Result<MessageResponse>.Failure("Invalid token.", 401);

        var userId = jwtService.GetUserIdFromToken(accessToken);
        if (userId == Guid.Empty)
            return Result<MessageResponse>.Failure("Invalid access token.", 401);

        var storedRefreshToken = await jwtService.GetRefreshTokenAsync(userId);
        if (storedRefreshToken == null || storedRefreshToken != refreshToken)
            return Result<MessageResponse>.Failure("Invalid refresh token.", 401);

        var user = await userRepository.GetByIdAsync(userId);
        if (user == null)
            return Result<MessageResponse>.Failure("Invalid refresh token.", 401);

        var newAccessToken = jwtService.GenerateToken(user);
        var newRefreshToken = jwtService.GenerateRefreshToken(user.Id);

        await jwtService.RevokeTokensAsync(userId, accessToken);
        await jwtService.SaveRefreshTokenAsync(userId, refreshToken);

        cookieManager.RemoveAuthCookies();
        cookieManager.SetAuthTokens(newAccessToken, newRefreshToken);

        return Result<MessageResponse>.Success(new MessageResponse("Refresh token successful"));
    }

    public async Task<Result<MessageResponse>> LoginAsync(LoginRequest request)
    {
        var user = await userRepository.GetByEmailAsync(request.Email);

        if (user is null || !argon2Hasher.VerifyPassword(request.Password, user.PasswordHash, user.PasswordSalt))
            return Result<MessageResponse>.Failure("Invalid email or password.", 401);

        var accessToken = jwtService.GenerateToken(user);
        var refreshToken = jwtService.GenerateRefreshToken(user.Id);

        cookieManager.SetAuthTokens(accessToken, refreshToken);

        await jwtService.SaveRefreshTokenAsync(user.Id, refreshToken);

        return Result<MessageResponse>.Success(new MessageResponse("Login successful"));
    }

    public async Task<Result<MessageResponse>> RegisterAsync(RegisterRequest request)
    {
        if (await userRepository.GetByEmailAsync(request.Email) is not null)
            return Result<MessageResponse>.Failure("Email already exists.", 400);

        var existingPendingUser = await redisRepository.GetAsync($"pending_user:{request.Email}");
        if (existingPendingUser != null)
            return Result<MessageResponse>.Failure(
                "A verification code has already been sent. Please check your email.", 400);

        var (hash, salt) = argon2Hasher.HashPassword(request.Password);

        var user = new User
        {
            Id = Guid.NewGuid(),
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            PasswordHash = hash,
            PasswordSalt = salt,
            Role = RoleType.User
        };

        var confirmationCode = CodeGenerator.GenerateConfirmationCode(6);

        var pendingUser = new PendingUserModel
        {
            User = user,
            ConfirmationCode = confirmationCode
        };

        var pendingUserJson = JsonSerializer.Serialize(pendingUser);

        await redisRepository.SaveAsync($"pending_user:{user.Email}", pendingUserJson, TimeSpan.FromMinutes(15));

        return await emailService.SendEmailConfirmationCodeAsync(user.Email, confirmationCode);
    }

    public async Task<Result<MessageResponse>> ConfirmEmailAsync(ConfirmEmailRequest request)
    {
        var pendingUserJson = await redisRepository.GetAsync($"pending_user:{request.Email}");

        if (pendingUserJson is null)
            return Result<MessageResponse>.Failure("Confirmation code expired or invalid.", 400);

        var pendingUser = JsonSerializer.Deserialize<PendingUserModel>(pendingUserJson);

        if (pendingUser is null ||
            !pendingUser.ConfirmationCode.Equals(request.ConfirmationCode, StringComparison.OrdinalIgnoreCase))
            return Result<MessageResponse>.Failure("Invalid confirmation code.", 400);

        await userRepository.AddAsync(pendingUser.User);

        await userRepository.SaveChangesAsync();

        await redisRepository.DeleteAsync($"pending_user:{request.Email}");

        return Result<MessageResponse>.Success(
            new MessageResponse("Email successfully verified. You can now log in."));
    }

    public async Task<Result<MessageResponse>> RequestPasswordResetAsync(PasswordResetRequest request)
    {
        var user = await userRepository.GetByEmailAsync(request.Email);
        if (user is null)
            return Result<MessageResponse>.Failure("Email not found", 400);

        var resetToken = CodeGenerator.GenerateConfirmationCode(6);
        var tokenEntry = new PasswordResetModel
        {
            Email = request.Email,
            Token = resetToken
        };

        var serializedToken = JsonSerializer.Serialize(tokenEntry);
        await redisRepository.SaveAsync($"password_reset:{user.Email}", serializedToken, TimeSpan.FromMinutes(15));

        return await emailService.SendPasswordResetEmailAsync(user.Email, resetToken);
    }

    public async Task<Result<MessageResponse>> ResetPasswordAsync(ResetPasswordRequest request)
    {
        var tokenJson = await redisRepository.GetAsync($"password_reset:{request.Email}");
        if (tokenJson is null)
            return Result<MessageResponse>.Failure("Invalid token.", 400);

        var tokenEntry = JsonSerializer.Deserialize<PasswordResetModel>(tokenJson);
        if (tokenEntry is null || tokenEntry.Token != request.Token)
            return Result<MessageResponse>.Failure("Invalid token.", 400);

        var (hash, salt) = argon2Hasher.HashPassword(request.Email);

        var user = await userRepository.GetByEmailAsync(request.Email);
        if (user == null)
            return Result<MessageResponse>.Failure("Email not found", 400);

        user.PasswordHash = hash;
        user.PasswordSalt = salt;

        userRepository.Update(user);

        await redisRepository.DeleteAsync($"password_reset:{user.Email}");

        return Result<MessageResponse>.Success(new MessageResponse("Password reset successfully."));
    }

    public async Task<Result<CurrentUserResponse>> GetCurrentUserAsync()
    {
        var accessToken = cookieManager.GetAccessToken();

        if (string.IsNullOrEmpty(accessToken))
            return Result<CurrentUserResponse>.Failure("Access token is invalid.", 401);

        var userId = jwtService.GetUserIdFromToken(accessToken);

        if (userId == Guid.Empty)
            return Result<CurrentUserResponse>.Failure("Access token is invalid.", 401);

        var user = await userRepository.GetByIdAsync(userId);

        if (user is null)
            return Result<CurrentUserResponse>.Failure("User does not exist.", 404);

        var currentUser = new CurrentUserResponse
        {
            Id = user.Id,
            Email = user.Email,
            Role = user.Role.ToString()
        };

        return Result<CurrentUserResponse>.Success(currentUser);
    }

    public async Task<Result<MessageResponse>> DeleteAccountAsync(Guid userId, string password)
    {
        var user = await userRepository.GetByIdAsync(userId);

        if (user is null)
            return Result<MessageResponse>.Failure("User does not exist.", 404);

        if (!argon2Hasher.VerifyPassword(password, user.PasswordHash, user.PasswordSalt))
            return Result<MessageResponse>.Failure("Invalid password.", 401);

        var accessToken = cookieManager.GetAccessToken();
        if (!string.IsNullOrEmpty(accessToken))
            await jwtService.RevokeTokensAsync(userId, accessToken);

        userRepository.Remove(user);
        await userRepository.SaveChangesAsync();

        cookieManager.RemoveAuthCookies();

        return Result<MessageResponse>.Success(new MessageResponse("Account successfully deleted."));
    }
}