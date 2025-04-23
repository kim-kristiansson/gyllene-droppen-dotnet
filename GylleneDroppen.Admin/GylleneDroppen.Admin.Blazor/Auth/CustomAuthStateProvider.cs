using System.Security.Claims;
using GylleneDroppen.Admin.Blazor.Services;
using GylleneDroppen.Application.Dtos.Auth;
using Microsoft.AspNetCore.Components.Authorization;

namespace GylleneDroppen.Admin.Blazor.Auth;

public class CustomAuthStateProvider(AuthService authService) : AuthenticationStateProvider
{
    private readonly TimeSpan _cacheTime = TimeSpan.FromMinutes(5);
    private readonly SemaphoreSlim _semaphore = new(1, 1);
    private CurrentUserResponse? _cachedUser;
    private DateTime _lastCheck = DateTime.MinValue;

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        return await GetAuthenticationStateInternalAsync();
    }

    private async Task<AuthenticationState> GetAuthenticationStateInternalAsync(bool forceRefresh = false)
    {
        try
        {
            // Use semaphore to prevent multiple simultaneous requests
            await _semaphore.WaitAsync();

            // Check if we need to refresh the cached user
            var now = DateTime.UtcNow;
            var needsRefresh = forceRefresh || _cachedUser == null || now - _lastCheck > _cacheTime;

            if (needsRefresh)
            {
                _cachedUser = await authService.GetCurrentUserAsync();
                _lastCheck = now;
            }

            // Create the authentication state
            if (_cachedUser != null)
            {
                // Create the claims for the authenticated user
                var claims = new List<Claim>
                {
                    new(ClaimTypes.Name, _cachedUser.Email),
                    new(ClaimTypes.NameIdentifier, _cachedUser.Id.ToString()),
                    new(ClaimTypes.Role, _cachedUser.Role)
                };

                var identity = new ClaimsIdentity(claims, "GylleneDroppenAuth");
                var user = new ClaimsPrincipal(identity);

                return new AuthenticationState(user);
            }
        }
        catch (Exception ex)
        {
            // On any error, clear the cached user
            _cachedUser = null;
            Console.WriteLine($"Authentication error: {ex.Message}");
        }
        finally
        {
            _semaphore.Release();
        }

        // Return anonymous user if not authenticated
        return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
    }

    public async Task<AuthenticationState> RefreshAuthenticationStateAsync()
    {
        var authState = await GetAuthenticationStateInternalAsync(true);
        NotifyAuthenticationStateChanged(Task.FromResult(authState));
        return authState;
    }
    
    public void NotifyUserLogout()
    {
        _cachedUser = null;
        _lastCheck = DateTime.MinValue;
        var anonymousUser = new ClaimsPrincipal(new ClaimsIdentity());
        var authState = Task.FromResult(new AuthenticationState(anonymousUser));
        NotifyAuthenticationStateChanged(authState);
    }
}