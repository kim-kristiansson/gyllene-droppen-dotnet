using System.Net;
using System.Net.Http.Json;
using GylleneDroppen.Admin.Blazor.Auth;
using GylleneDroppen.Application.Dtos.Auth;
using GylleneDroppen.Application.Dtos.Common;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace GylleneDroppen.Admin.Blazor.Services;

public class AuthService(
    IHttpClientFactory httpClientFactory,
    NavigationManager navigationManager,
    ILogger<AuthService> logger,
    AppConfigService configService,
    AuthenticationStateProvider authStateProvider)
{
    private readonly CustomAuthStateProvider _authStateProvider = (CustomAuthStateProvider)authStateProvider;
    private string _apiBaseUrl = string.Empty;
    private bool _isInitialized;

    // Cast to specific implementation

    private async Task<HttpClient> GetHttpClientAsync()
    {
        if (!_isInitialized)
        {
            _apiBaseUrl = await configService.GetApiBaseUrlAsync();
            _isInitialized = true;
        }

        var client = httpClientFactory.CreateClient("AuthAPI");
        client.BaseAddress = new Uri(_apiBaseUrl);
        return client;
    }

    public async Task<AuthResult> LoginAsync(LoginRequest request)
    {
        try
        {
            var httpClient = await GetHttpClientAsync();

            logger.LogInformation("Attempting login for user: {Email}", request.Email);
            var response = await httpClient.PostAsJsonAsync("api/auth/login", request);

            if (response.IsSuccessStatusCode)
            {
                logger.LogInformation("Login successful for user: {Email}", request.Email);

                // Refresh the authentication state
                await _authStateProvider.RefreshAuthenticationStateAsync();

                return new AuthResult { Success = true };
            }

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                logger.LogWarning("Unauthorized login attempt for user: {Email}", request.Email);
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
                logger.LogError(ex, "Failed to parse error response");
            }

            logger.LogWarning("Login failed for user: {Email}, Status: {StatusCode}",
                request.Email, response.StatusCode);

            return new AuthResult
            {
                Success = false,
                ErrorMessage = errorMessage
            };
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Exception during login for user: {Email}", request.Email);
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
            var httpClient = await GetHttpClientAsync();

            var response = await httpClient.GetAsync("api/auth/current-user");

            if (response.IsSuccessStatusCode)
            {
                var user = await response.Content.ReadFromJsonAsync<CurrentUserResponse>();
                logger.LogInformation("Retrieved current user: {Email}, Role: {Role}",
                    user?.Email, user?.Role);
                return user;
            }

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                logger.LogInformation("User is not authenticated");
                return null;
            }

            logger.LogWarning("Failed to get current user. Status: {StatusCode}", response.StatusCode);
            return null;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Exception while getting current user");
            return null;
        }
    }

    public async Task<bool> LogoutAsync()
    {
        try
        {
            var httpClient = await GetHttpClientAsync();

            logger.LogInformation("Logging out current user");
            await httpClient.PostAsync("api/auth/logout", null);

            // Clear the authentication state
            _authStateProvider.NotifyUserLogout();

            // Always redirect to login page
            navigationManager.NavigateTo("/login", true);
            return true;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Exception during logout");

            // Still redirect to login even if API call fails
            navigationManager.NavigateTo("/login", true);
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