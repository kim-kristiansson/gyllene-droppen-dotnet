using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using GylleneDroppen.Admin.Api.Options;
using GylleneDroppen.Admin.Api.Repositories.Interfaces;
using GylleneDroppen.Admin.Api.Services.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace GylleneDroppen.Admin.Api.Services;

public class JwtService(IOptions<JwtOptions> jwtOptions, IRedisRepository redisRepository) : IJwtService
{
    public string GenerateToken(Guid userId)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Value.Secret));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new (ClaimTypes.Name, userId.ToString()),
            new (JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
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
    
    public async Task BlacklistTokenAsync(string token)
    {
        var handler = new JwtSecurityTokenHandler();

        if (handler.ReadToken(token) is not JwtSecurityToken jwtToken) return;

        var expiration = jwtToken.ValidTo;
        var ttl = expiration.Subtract(DateTime.UtcNow);

        if (ttl <= TimeSpan.Zero) return;

        await redisRepository.SaveAsync($"blacklist:{token}", "revoked", ttl);
    }

    public async Task<bool> IsTokenBlacklistedAsync(string token)
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
        await redisRepository.SaveAsync($"refresh:{userId}", refreshToken, TimeSpan.FromDays(jwtOptions.Value.RefreshTokenExpirationDays));
    }

    public async Task<string?> GetRefreshTokenAsync(Guid userId)
    {
        return await redisRepository.GetAsync($"refresh:{userId}");
    }

    public async Task RevokeRefreshTokenAsync(Guid userId)
    {
        await redisRepository.DeleteAsync($"refresh:{userId}");
    }
}