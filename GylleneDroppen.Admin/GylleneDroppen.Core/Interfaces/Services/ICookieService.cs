namespace GylleneDroppen.Core.Interfaces.Services;

public interface ICookieService
{
    void SetAccessToken(string accessToken);
    void SetRefreshToken(string refreshToken);
    string? GetAccessToken();
    string? GetRefreshToken();
    void SetAuthTokens(string accessToken, string refreshToken);
    void RemoveAuthCookies();
}