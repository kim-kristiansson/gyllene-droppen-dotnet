namespace GylleneDroppen.Admin.Api.Services.Interfaces;

public interface IJwtService
{
    string GenerateToken(Guid userId);
    Task BlacklistTokenAsync(string token);
    Task<bool> IsTokenBlacklistedAsync(string token);
    string GenerateRefreshToken(Guid userId);
    Task SaveRefreshTokenAsync(Guid userId, string refreshToken);
    Task<string?> GetRefreshTokenAsync(Guid userId);
    Task RevokeRefreshTokenAsync(Guid userId);
}