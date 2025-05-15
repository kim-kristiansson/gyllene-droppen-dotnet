using GylleneDroppen.Domain.Entities;

namespace GylleneDroppen.Application.Services.Shared;

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