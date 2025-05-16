// GylleneDroppen.Blazor.Client/Authentication/ApiAuthenticationStateProvider.cs

using System.Security.Claims;
using GylleneDroppen.Application.Dtos.Shared.Auth;
using GylleneDroppen.Blazor.Client.Services;
using Microsoft.AspNetCore.Components.Authorization;

namespace GylleneDroppen.Blazor.Client.Authentication;

public class ApiAuthenticationStateProvider(AuthService authService) : AuthenticationStateProvider
{
    private CurrentUserResponse? _cachedUser;

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var user = await GetUserAsync();

        if (user == null) return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, user.Email),
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Role, user.Role)
        };

        var identity = new ClaimsIdentity(claims, "api");
        return new AuthenticationState(new ClaimsPrincipal(identity));
    }

    private async Task<CurrentUserResponse?> GetUserAsync()
    {
        if (_cachedUser != null)
            return _cachedUser;

        _cachedUser = await authService.GetCurrentUserAsync();
        return _cachedUser;
    }

    public void NotifyUserAuthentication(CurrentUserResponse user)
    {
        _cachedUser = user;
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }

    public void NotifyUserLogout()
    {
        _cachedUser = null;
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }
}