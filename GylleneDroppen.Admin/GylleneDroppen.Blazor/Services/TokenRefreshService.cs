using System.Net;
using Microsoft.AspNetCore.Components;
using Timer = System.Timers.Timer;

namespace GylleneDroppen.Blazor.Services;

public class TokenRefreshService : IDisposable
{
    private const int TokenRefreshIntervalMinutes = 10;
    private readonly AuthService _authService;
    private readonly HttpClient _httpClient;
    private readonly ILogger<TokenRefreshService> _logger;
    private readonly NavigationManager _navigationManager;
    private readonly SemaphoreSlim _refreshSemaphore = new(1, 1);
    private readonly Timer _tokenRefreshTimer;
    private bool _isRefreshing;

    public TokenRefreshService(
        HttpClient httpClient,
        NavigationManager navigationManager,
        ILogger<TokenRefreshService> logger,
        AuthService authService)
    {
        _httpClient = httpClient;
        _navigationManager = navigationManager;
        _logger = logger;
        _authService = authService;

        _tokenRefreshTimer = new Timer(TokenRefreshIntervalMinutes * 60 * 1000);
        _tokenRefreshTimer.Elapsed += async (sender, e) => await RefreshTokenIfNeededAsync();
        _tokenRefreshTimer.AutoReset = true;
        _tokenRefreshTimer.Start();
    }

    public void Dispose()
    {
        _tokenRefreshTimer?.Dispose();
        _refreshSemaphore?.Dispose();
    }

    public async Task Initialize()
    {
        await GetUserToEnsureInitialization();
        await RefreshTokenIfNeededAsync();
    }

    private async Task GetUserToEnsureInitialization()
    {
        try
        {
            await _authService.GetCurrentUserAsync();
            _logger.LogInformation("Service initialization completed");
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to initialize services");
        }
    }

    public async Task<bool> RefreshTokenIfNeededAsync()
    {
        try
        {
            if (_isRefreshing)
                return false;

            await _refreshSemaphore.WaitAsync();
            _isRefreshing = true;

            // Try refreshing the token
            _logger.LogInformation("Attempting to refresh token");
            var response = await _httpClient.PostAsync("api/auth/refresh-token", null);

            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation("Token refreshed successfully");
                return true;
            }

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                _logger.LogWarning("Token refresh failed - unauthorized");
                _navigationManager.NavigateTo("/login", true);
                return false;
            }

            _logger.LogWarning("Token refresh failed: {StatusCode}", response.StatusCode);
            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error refreshing token");
            return false;
        }
        finally
        {
            _isRefreshing = false;
            _refreshSemaphore.Release();
        }
    }
}