using System.Net.Http.Json;
using GylleneDroppen.Application.Dtos.Shared.Auth;
using GylleneDroppen.Blazor.Client.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Http;

namespace GylleneDroppen.Blazor.Client.Services;

public class AuthService
{
    private readonly ApiAuthenticationStateProvider _authStateProvider;
    private readonly HttpClient _httpClient;

    public AuthService(HttpClient httpClient, ApiAuthenticationStateProvider authStateProvider)
    {
        _httpClient = httpClient;
        _authStateProvider = authStateProvider;
    }

    public async Task<bool> LoginAsync(LoginRequest loginRequest)
    {
        try
        {
            // Create a message with explicit credential inclusion
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, "api/auth/login")
            {
                Content = JsonContent.Create(loginRequest)
            };

            // Ensure credentials are included
            requestMessage.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);

            // Set credentials inclusion
            var response = await _httpClient.SendAsync(requestMessage);

            Console.WriteLine($"Login status code: {response.StatusCode}");

            if (!response.IsSuccessStatusCode) return false;

            // Get the current user data immediately after successful login
            var user = await GetCurrentUserAsync();
            if (user == null) return false;

            // Notify the authentication state provider
            _authStateProvider.NotifyUserAuthentication(user);
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Login error: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> LogoutAsync()
    {
        try
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, "api/auth/logout");
            requestMessage.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);

            var response = await _httpClient.SendAsync(requestMessage);

            // Always notify the auth state provider of logout, regardless of response
            _authStateProvider.NotifyUserLogout();

            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Logout error: {ex.Message}");
            // Still notify logout even if there's an error with the request
            _authStateProvider.NotifyUserLogout();
            return false;
        }
    }

    public async Task<CurrentUserResponse?> GetCurrentUserAsync()
    {
        try
        {
            // Create a message with explicit credential inclusion
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, "api/auth/current-user");

            // Ensure credentials are included
            requestMessage.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);

            var response = await _httpClient.SendAsync(requestMessage);

            if (response.IsSuccessStatusCode)
            {
                var user = await response.Content.ReadFromJsonAsync<CurrentUserResponse>();
                if (user != null)
                {
                    _authStateProvider.NotifyUserAuthentication(user);
                    return user;
                }
            }

            return null;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"GetCurrentUser error: {ex.Message}");
            return null;
        }
    }

    public async Task<bool> RequestPasswordResetAsync(string email)
    {
        try
        {
            var resetRequest = new PasswordResetRequest { Email = email };
            var response = await _httpClient.PostAsJsonAsync("api/auth/request-password-reset", resetRequest);

            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"RequestPasswordReset error: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> ResetPasswordAsync(string email, string token, string newPassword)
    {
        try
        {
            var resetRequest = new ResetPasswordRequest
            {
                Email = email,
                Token = token,
                NewPassword = newPassword
            };

            var response = await _httpClient.PostAsJsonAsync("api/auth/reset-password", resetRequest);

            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"ResetPassword error: {ex.Message}");
            return false;
        }
    }
}

// Define the request models if needed (you may already have these elsewhere)
public class PasswordResetRequest
{
    public required string Email { get; set; }
}

public class ResetPasswordRequest
{
    public required string Email { get; set; }
    public required string Token { get; set; }
    public required string NewPassword { get; set; }
}