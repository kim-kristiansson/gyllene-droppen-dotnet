using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using GylleneDroppen.Application.Interfaces.Repositories.Shared;
using GylleneDroppen.Application.Services.Shared;
using GylleneDroppen.Domain.Entities;
using GylleneDroppen.Infrastructure.Settings;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace GylleneDroppen.Infrastructure.Authentication;

public class JwtService(IOptions<JwtSettings> jwtSettings, IRedisRepository redisRepository) : IJwtService
{
    public async Task BlacklistAccessTokenAsync(string token)
    {
        var handler = new JwtSecurityTokenHandler();

        if (handler.ReadToken(token) is not JwtSecurityToken jwtToken) return;

        var expiration = jwtToken.ValidTo;
        var ttl = expiration.Subtract(DateTime.UtcNow);

        if (ttl <= TimeSpan.Zero) return;

        await redisRepository.SaveAsync($"blacklist:{token}", "revoked", ttl);
    }

    public async Task<bool> IsAccessTokenBlacklistedAsync(string token)
    {
        return await redisRepository.ExistsAsync($"blacklist:{token}");
    }

    public string GenerateRefreshToken(Guid userId)
    {
        var randomBytes = new byte[62];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomBytes);
        return Convert.ToBase64String(randomBytes);
    }

    public async Task SaveRefreshTokenAsync(Guid userId, string refreshToken)
    {
        await redisRepository.SaveAsync($"refresh:{userId}", refreshToken,
            TimeSpan.FromDays(jwtSettings.Value.RefreshTokenExpirationDays));
    }

    public async Task<string?> GetRefreshTokenAsync(Guid userId)
    {
        return await redisRepository.GetAsync($"refresh:{userId}");
    }

    public async Task RevokeTokensAsync(Guid userId, string accessToken)
    {
        await redisRepository.DeleteAsync($"refresh:{userId}");
        await BlacklistAccessTokenAsync(accessToken);
    }

    public Guid GetUserIdFromToken(string token)
    {
        var handler = new JwtSecurityTokenHandler();

        if (handler.ReadToken(token) is not JwtSecurityToken jwtToken)
            return Guid.Empty;

        var userIdClaim = jwtToken.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;

        return Guid.TryParse(userIdClaim, out var userId) ? userId : Guid.Empty;
    }

    public string GenerateToken(User user)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Value.Secret));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, user.Email),
            new(ClaimTypes.Role, user.Role.ToString()),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var token = new JwtSecurityToken(
            jwtSettings.Value.Issuer,
            jwtSettings.Value.Audience,
            claims,
            expires: DateTime.Now.AddMinutes(jwtSettings.Value.AccessTokenExpirationMinutes),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}