using GylleneDroppen.Api.Models;

namespace GylleneDroppen.Api.Services.Interfaces;

public interface IJwtService
{
    string GenerateToken(User user);
    Task BlacklistAccessTokenAsync(string token);
    Task<bool> IsAccessTokenBlacklistedAsync(string token);
    string GenerateRefreshToken(Guid userId);
    Task SaveRefreshTokenAsync(Guid userId, string refreshToken);
    Task<string?> GetRefreshTokenAsync(Guid userId);
    Task RevokeTokensAsync(Guid userId, string accessToken);
    Guid GetUserIdFromToken(string token);
}