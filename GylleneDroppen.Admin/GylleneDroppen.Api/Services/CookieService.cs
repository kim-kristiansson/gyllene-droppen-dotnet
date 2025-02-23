using GylleneDroppen.Api.Options;
using GylleneDroppen.Api.Services.Interfaces;
using Microsoft.Extensions.Options;

namespace GylleneDroppen.Api.Services;

public class CookieService(IHttpContextAccessor httpContextAccessor, IOptions<JwtOptions> jwtOptions) : ICookieService
{
    public void SetAccessToken(string token)
    {
        var options = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.None,
            Expires = DateTime.UtcNow.AddMinutes(jwtOptions.Value.AccessTokenExpirationMinutes),
            Path = "/"
        };
        
        httpContextAccessor.HttpContext?.Response.Cookies.Append("accessToken", token, options);
    }

    public void SetRefreshToken(string token)
    {
        var options = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.None,
            Expires = DateTime.UtcNow.AddDays(jwtOptions.Value.RefreshTokenExpirationDays),
            Path = "/"
        };
        
        httpContextAccessor.HttpContext?.Response.Cookies.Append("refreshToken", token, options);
    }

    public void RemoveAuthCookies()
    {
        httpContextAccessor.HttpContext?.Response.Cookies.Delete("accessToken");
        httpContextAccessor.HttpContext?.Response.Cookies.Delete("refreshToken");
    }
}