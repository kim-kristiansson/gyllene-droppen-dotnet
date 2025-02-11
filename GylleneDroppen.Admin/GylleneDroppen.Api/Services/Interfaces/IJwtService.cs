using GylleneDroppen.Api.Models;

namespace GylleneDroppen.Api.Services.Interfaces;

public interface IJwtService
{
    string GenerateToken(User user);
    Task BlacklistTokenAsync(string token);
    Task<bool> IsTokenBlacklistedAsync(string token);
    string GenerateRefreshToken(Guid userId);
    Task SaveRefreshTokenAsync(Guid userId, string refreshToken);
    Task<string?> GetRefreshTokenAsync(Guid userId);
    Task RevokeRefreshTokenAsync(Guid userId);
}