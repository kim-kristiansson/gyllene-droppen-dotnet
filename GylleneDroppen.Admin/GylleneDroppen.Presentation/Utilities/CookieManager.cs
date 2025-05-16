using GylleneDroppen.Application.Interfaces.Utilities;
using GylleneDroppen.Infrastructure.Settings;
using Microsoft.Extensions.Options;

namespace GylleneDroppen.Presentation.Utilities;

public class CookieManager(IHttpContextAccessor httpContextAccessor, IOptions<JwtSettings> jwtSettings) : ICookieManager
{
    private const string AccessTokenKey = "accessToken";
    private const string RefreshTokenKey = "refreshToken";

    public string? GetAccessToken()
    {
        return httpContextAccessor.HttpContext?.Request.Cookies[AccessTokenKey];
    }

    public string? GetRefreshToken()
    {
        return httpContextAccessor.HttpContext?.Request.Cookies[RefreshTokenKey];
    }

    public void RemoveAuthCookies()
    {
        httpContextAccessor.HttpContext?.Response.Cookies.Delete(AccessTokenKey);
        httpContextAccessor.HttpContext?.Response.Cookies.Delete(RefreshTokenKey);
    }

    public void SetAuthTokens(string accessToken, string refreshToken)
    {
        SetAccessToken(accessToken);
        SetRefreshToken(refreshToken);
    }

    private void SetAccessToken(string token)
    {
        var options = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.None,
            Expires = DateTime.UtcNow.AddMinutes(jwtSettings.Value.AccessTokenExpirationMinutes),
            Path = "/"
        };

        httpContextAccessor.HttpContext?.Response.Cookies.Append(AccessTokenKey, token, options);
    }

    private void SetRefreshToken(string token)
    {
        var options = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.None,
            Expires = DateTime.UtcNow.AddDays(jwtSettings.Value.RefreshTokenExpirationDays),
            Path = "/"
        };

        httpContextAccessor.HttpContext?.Response.Cookies.Append(RefreshTokenKey, token, options);
    }
}