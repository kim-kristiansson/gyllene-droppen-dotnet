using System.Net.Http.Json;
using GylleneDroppen.Application.Dtos.Shared.Auth;
using GylleneDroppen.Blazor.Client.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Http;

namespace GylleneDroppen.Blazor.Client.Services;

public class AuthService(HttpClient httpClient, ApiAuthenticationStateProvider authStateProvider)
{
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
            var response = await httpClient.SendAsync(requestMessage);

            Console.WriteLine($"Login status code: {response.StatusCode}");

            if (!response.IsSuccessStatusCode) return false;
            // Get the current user data immediately after successful login
            var user = await GetCurrentUserAsync();
            if (user == null) return false;
            // Notify the authentication state provider
            authStateProvider.NotifyUserAuthentication(user);
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Login error: {ex.Message}");
            return false;
        }
    }

// Update to your AuthService.cs LogoutAsync method
    public async Task<bool> LogoutAsync()
    {
        try
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, "api/auth/logout");
            requestMessage.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);

            var response = await httpClient.SendAsync(requestMessage);

            // Always notify the auth state provider of logout, regardless of response
            authStateProvider.NotifyUserLogout();

            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Logout error: {ex.Message}");
            // Still notify logout even if there's an error with the request
            authStateProvider.NotifyUserLogout();
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

            var response = await httpClient.SendAsync(requestMessage);

            if (response.IsSuccessStatusCode) return await response.Content.ReadFromJsonAsync<CurrentUserResponse>();

            return null;
        }
        catch
        {
            return null;
        }
    }

    // Add the method to request a password reset
    public async Task<bool> RequestPasswordResetAsync(string email)
    {
        try
        {
            var resetRequest = new PasswordResetRequest { Email = email };
            var response = await httpClient.PostAsJsonAsync("api/auth/request-password-reset", resetRequest);

            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"RequestPasswordReset error: {ex.Message}");
            return false;
        }
    }

    // Add the method to reset a password with a token
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

            var response = await httpClient.PostAsJsonAsync("api/auth/reset-password", resetRequest);

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