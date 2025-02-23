namespace GylleneDroppen.Api.Services.Interfaces;

public interface ICookieService
{
    void SetAccessToken(string accessToken);
    void SetRefreshToken(string refreshToken);
    void RemoveAuthCookies();
}