using System.Security.Claims;
using GylleneDroppen.Admin.Blazor.Services;
using GylleneDroppen.Application.Dtos.Auth;
using Microsoft.AspNetCore.Components.Authorization;

namespace GylleneDroppen.Admin.Blazor.Auth;

public class CustomAuthStateProvider : AuthenticationStateProvider
{
    private readonly TimeSpan _cacheTime = TimeSpan.FromMinutes(5);
    private readonly AppConfigService _configService;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly SemaphoreSlim _semaphore = new(1, 1);
    private string _apiBaseUrl = string.Empty;
    private CurrentUserResponse? _cachedUser;
    private bool _isInitialized;
    private DateTime _lastCheck = DateTime.MinValue;

    public CustomAuthStateProvider(IHttpClientFactory httpClientFactory, AppConfigService configService)
    {
        _httpClientFactory = httpClientFactory;
        _configService = configService;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        return await GetAuthenticationStateInternalAsync();
    }

    private async Task<HttpClient> GetHttpClientAsync()
    {
        if (!_isInitialized)
        {
            _apiBaseUrl = await _configService.GetApiBaseUrlAsync();
            _isInitialized = true;
        }

        var client = _httpClientFactory.CreateClient("AuthAPI");
        client.BaseAddress = new Uri(_apiBaseUrl);
        return client;
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
                try
                {
                    var httpClient = await GetHttpClientAsync();
                    var response = await httpClient.GetAsync("api/auth/current-user");

                    if (response.IsSuccessStatusCode)
                    {
                        _cachedUser = await response.Content.ReadFromJsonAsync<CurrentUserResponse>();
                        _lastCheck = now;
                    }
                    else
                    {
                        _cachedUser = null;
                    }
                }
                catch
                {
                    _cachedUser = null;
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