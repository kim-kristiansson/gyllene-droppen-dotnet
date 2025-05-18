using System.Security.Claims;
using GylleneDroppen.Application.Dtos.Shared.Auth;
using Microsoft.AspNetCore.Components.Authorization;

namespace GylleneDroppen.Blazor.Client.Authentication;

public class ApiAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly ClaimsPrincipal _anonymous = new(new ClaimsIdentity());
    private CurrentUserResponse? _cachedUser;

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        // If we have a cached user, return an authenticated state
        if (_cachedUser != null)
        {
            var claims = new List<Claim>
            {
                new(ClaimTypes.Name, _cachedUser.Email),
                new(ClaimTypes.NameIdentifier, _cachedUser.Id.ToString()),
                new(ClaimTypes.Role, _cachedUser.Role)
            };

            var identity = new ClaimsIdentity(claims, "jwt");
            var user = new ClaimsPrincipal(identity);
            return new AuthenticationState(user);
        }

        // Otherwise return an unauthenticated state
        return new AuthenticationState(_anonymous);
    }

    public void NotifyUserAuthentication(CurrentUserResponse user)
    {
        _cachedUser = user;

        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, user.Email),
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Role, user.Role)
        };

        var identity = new ClaimsIdentity(claims, "jwt");
        var authenticatedUser = new ClaimsPrincipal(identity);

        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(authenticatedUser)));
    }

    public void NotifyUserLogout()
    {
        _cachedUser = null;
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_anonymous)));
    }
}