using System.Net;
using System.Net.Http.Json;
using GylleneDroppen.Application.Dtos.Auth;
using GylleneDroppen.Application.Dtos.Common;
using Microsoft.AspNetCore.Components;

namespace GylleneDroppen.Admin.Blazor.Services;

public class AuthService
{
    private readonly AppConfigService _configService;
    private readonly HttpClient _httpClient;
    private readonly ILogger<AuthService> _logger;
    private readonly NavigationManager _navigationManager;

    private bool _isInitialized;

    public AuthService(
        HttpClient httpClient,
        NavigationManager navigationManager,
        ILogger<AuthService> logger,
        AppConfigService configService)
    {
        _httpClient = httpClient;
        _navigationManager = navigationManager;
        _logger = logger;
        _configService = configService;
    }

    private async Task EnsureInitializedAsync()
    {
        if (_isInitialized)
            return;

        var apiBaseUrl = await _configService.GetApiBaseUrlAsync();
        _httpClient.BaseAddress = new Uri(apiBaseUrl);
        _isInitialized = true;
    }

    public async Task<AuthResult> LoginAsync(LoginRequest request)
    {
        try
        {
            await EnsureInitializedAsync();

            _logger.LogInformation("Attempting login for user: {Email}", request.Email);
            var response = await _httpClient.PostAsJsonAsync("api/auth/login", request);

            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation("Login successful for user: {Email}", request.Email);
                return new AuthResult { Success = true };
            }

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                _logger.LogWarning("Unauthorized login attempt for user: {Email}", request.Email);
                return new AuthResult
                {
                    Success = false,
                    ErrorMessage = "Felaktiga inloggningsuppgifter. Kontrollera e-post och lösenord."
                };
            }

            var errorMessage = "Inloggningen misslyckades.";
            try
            {
                var errorResponse = await response.Content.ReadFromJsonAsync<MessageResponse>();
                if (errorResponse != null && !string.IsNullOrEmpty(errorResponse.Message))
                    errorMessage = errorResponse.Message;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to parse error response");
            }

            _logger.LogWarning("Login failed for user: {Email}, Status: {StatusCode}",
                request.Email, response.StatusCode);

            return new AuthResult
            {
                Success = false,
                ErrorMessage = errorMessage
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception during login for user: {Email}", request.Email);
            return new AuthResult
            {
                Success = false,
                ErrorMessage = "Ett tekniskt fel inträffade. Försök igen senare."
            };
        }
    }

    public async Task<CurrentUserResponse?> GetCurrentUserAsync()
    {
        try
        {
            await EnsureInitializedAsync();

            var response = await _httpClient.GetAsync("api/auth/current-user");

            if (response.IsSuccessStatusCode)
            {
                var user = await response.Content.ReadFromJsonAsync<CurrentUserResponse>();
                _logger.LogInformation("Retrieved current user: {Email}, Role: {Role}",
                    user?.Email, user?.Role);
                return user;
            }

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                _logger.LogInformation("User is not authenticated");
                return null;
            }

            _logger.LogWarning("Failed to get current user. Status: {StatusCode}", response.StatusCode);
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception while getting current user");
            return null;
        }
    }

    public async Task<bool> LogoutAsync()
    {
        try
        {
            await EnsureInitializedAsync();

            _logger.LogInformation("Logging out current user");
            await _httpClient.PostAsync("api/auth/logout", null);

            // Always redirect to login page
            _navigationManager.NavigateTo("/login", true);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception during logout");

            // Still redirect to login even if API call fails
            _navigationManager.NavigateTo("/login", true);
            return false;
        }
    }

    public async Task<bool> IsAuthenticatedAdmin()
    {
        var user = await GetCurrentUserAsync();
        return user != null && user.Role == "Admin";
    }
}

public class AuthResult
{
    public bool Success { get; set; }
    public string ErrorMessage { get; set; } = string.Empty;
}