using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using GylleneDroppen.Admin.Api.Options;
using GylleneDroppen.Admin.Api.Utilities.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace GylleneDroppen.Admin.Api.Utilities;

public class JsonWebToken(IOptions<JwtOptions> jwtOptions) : IJsonWebToken
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
            expires: DateTime.Now.AddMinutes(jwtOptions.Value.ExpirationMinutes),
            signingCredentials: credentials
        );
        
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}