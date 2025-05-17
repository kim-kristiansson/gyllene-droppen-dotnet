using System.Security.Claims;
using GylleneDroppen.Application.Dtos.Shared.Auth;
using Microsoft.AspNetCore.Components.Authorization;

namespace GylleneDroppen.Blazor.Client.Authentication;

public class ApiAuthenticationStateProvider : AuthenticationStateProvider
{
    private CurrentUserResponse? _cachedUser;

    // Remove the AuthService dependency

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        if (_cachedUser == null)
            // Return an empty, unauthenticated state if no user is cached
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));

        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, _cachedUser.Email),
            new(ClaimTypes.NameIdentifier, _cachedUser.Id.ToString()),
            new(ClaimTypes.Role, _cachedUser.Role)
        };

        var identity = new ClaimsIdentity(claims, "api");
        return new AuthenticationState(new ClaimsPrincipal(identity));
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