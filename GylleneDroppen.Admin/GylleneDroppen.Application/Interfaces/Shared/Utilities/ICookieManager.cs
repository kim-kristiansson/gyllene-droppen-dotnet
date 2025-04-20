namespace GylleneDroppen.Application.Interfaces.Shared.Utilities;

public interface ICookieManager
{
    void SetAccessToken(string accessToken);
    void SetRefreshToken(string refreshToken);
    string? GetAccessToken();
    string? GetRefreshToken();
    void SetAuthTokens(string accessToken, string refreshToken);
    void RemoveAuthCookies();
}