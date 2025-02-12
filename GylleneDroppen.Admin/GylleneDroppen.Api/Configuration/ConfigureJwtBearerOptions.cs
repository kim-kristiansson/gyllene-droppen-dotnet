using System.Text;
using GylleneDroppen.Api.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace GylleneDroppen.Api.Configuration;

public class ConfigureJwtBearerOptions : IConfigureOptions<JwtBearerOptions>
{
    private readonly JwtOptions _jwtOptions;

    public ConfigureJwtBearerOptions(IOptions<JwtOptions> jwtOptions)
    {
        _jwtOptions = jwtOptions.Value;

        if (string.IsNullOrWhiteSpace(_jwtOptions.Secret) || _jwtOptions.Secret.Length < 32)
        {
            throw new ArgumentException("JWT Secret is missing or too short (must be at least 32 characters).");
        }
    }

    public void Configure(JwtBearerOptions options)
    {
        Console.WriteLine($"JWT Secret!!: {_jwtOptions.Secret}");
        options.TokenValidationParameters = new TokenValidationParameters
        {
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Secret)),
            ValidIssuer = _jwtOptions.Issuer,
            ValidAudience = _jwtOptions.Audience,
            ValidateIssuerSigningKey = true,
            ValidateIssuer = !string.IsNullOrWhiteSpace(_jwtOptions.Issuer),
            ValidateAudience = !string.IsNullOrWhiteSpace(_jwtOptions.Audience),
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
    }
}