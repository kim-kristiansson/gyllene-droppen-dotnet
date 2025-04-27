using System.Net;
using System.Net.Http.Json;
using GylleneDroppen.Application.Dtos.Auth;
using GylleneDroppen.Application.Dtos.Common;
using Microsoft.AspNetCore.Components;

namespace GylleneDroppen.Blazor.Services;

public class AuthService(
    HttpClient httpClient,
    NavigationManager navigationManager,
    ILogger<AuthService> logger)
    : BaseApiService(httpClient, navigationManager, logger)
{
    public async Task<LoginResult> LoginAsync(string email, string password)
    {
        try
        {
            await EnsureInitializedAsync();

            Logger.LogInformation("Attempting login for user: {Email}", email);

            var loginRequest = new LoginRequest
            {
                Email = email,
                Password = password
            };

            var response = await HttpClient.PostAsJsonAsync("api/auth/login", loginRequest);

            if (response.IsSuccessStatusCode)
            {
                Logger.LogInformation("Login successful for user: {Email}", email);
                return new LoginResult
                {
                    Success = true
                };
            }

            string errorMessage;

            switch (response.StatusCode)
            {
                case HttpStatusCode.Unauthorized:
                    errorMessage = "Felaktiga inloggningsuppgifter. Kontrollera e-post och lösenord.";
                    Logger.LogWarning("Unauthorized login attempt for user: {Email}", email);
                    break;
                case HttpStatusCode.BadRequest:
                    try
                    {
                        var errorResponse = await response.Content.ReadFromJsonAsync<MessageResponse>();
                        errorMessage = errorResponse?.Message ?? "Ett fel uppstod vid inloggningen.";
                    }
                    catch
                    {
                        errorMessage = "Ett fel uppstod vid inloggningen.";
                    }

                    Logger.LogWarning("Login failed for user: {Email}, Error: {Error}", email, errorMessage);
                    break;
                default:
                    errorMessage = "Ett tekniskt fel uppstod. Vänligen försök igen senare.";
                    Logger.LogWarning("Login failed for user: {Email}, Status: {StatusCode}", email,
                        response.StatusCode);
                    break;
            }

            return new LoginResult
            {
                Success = false,
                ErrorMessage = errorMessage
            };
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Exception during login for user: {Email}", email);
            return new LoginResult
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

            var response = await HttpClient.GetAsync("api/auth/current-user");

            if (response.IsSuccessStatusCode)
            {
                var user = await response.Content.ReadFromJsonAsync<CurrentUserResponse>();
                Logger.LogInformation("Retrieved current user: {Email}, Role: {Role}", user?.Email, user?.Role);
                return user;
            }

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                Logger.LogInformation("User is not authenticated");
                return null;
            }

            Logger.LogWarning("Failed to get current user. Status: {StatusCode}", response.StatusCode);
            return null;
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Exception while getting current user");
            return null;
        }
    }

    public async Task<bool> LogoutAsync()
    {
        try
        {
            await EnsureInitializedAsync();

            Logger.LogInformation("Logging out current user");
            await HttpClient.PostAsync("api/auth/logout", null);

            // Always redirect to login page
            NavigationManager.NavigateTo("/login", true);
            return true;
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Exception during logout");

            // Still redirect to login even if API call fails
            NavigationManager.NavigateTo("/login", true);
            return false;
        }
    }
}

public class LoginResult
{
    public bool Success { get; set; }
    public string ErrorMessage { get; set; } = string.Empty;
}