namespace GylleneDroppen.Application.Interfaces.Utilities;

public interface ICookieManager
{
    string? GetAccessToken();
    string? GetRefreshToken();
    void SetAuthTokens(string accessToken, string refreshToken);
    void RemoveAuthCookies();
}