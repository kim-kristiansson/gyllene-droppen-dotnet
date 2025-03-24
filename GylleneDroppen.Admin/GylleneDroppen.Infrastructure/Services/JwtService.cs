using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using GylleneDroppen.Api.Options;
using GylleneDroppen.Core.Entities;
using GylleneDroppen.Core.Interfaces.Repositories;
using GylleneDroppen.Core.Interfaces.Services;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace GylleneDroppen.Infrastructure.Services;

public class JwtService(IOptions<JwtOptions> jwtOptions, IRedisRepository redisRepository) : IJwtService
{
    public string GenerateToken(User user)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Value.Secret));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, user.Email),
            new(ClaimTypes.Role, user.Role.ToString()),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var token = new JwtSecurityToken(
            issuer: jwtOptions.Value.Issuer,
            audience: jwtOptions.Value.Audience,
            claims: claims,
            expires: DateTime.Now.AddMinutes(jwtOptions.Value.AccessTokenExpirationMinutes),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

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
            TimeSpan.FromDays(jwtOptions.Value.RefreshTokenExpirationDays));
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
}