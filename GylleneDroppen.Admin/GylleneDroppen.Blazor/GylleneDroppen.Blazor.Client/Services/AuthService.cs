using System.Net.Http.Json;
using GylleneDroppen.Application.Dtos.Shared.Auth;
using Microsoft.AspNetCore.Components.WebAssembly.Http;
using Microsoft.JSInterop;

namespace GylleneDroppen.Blazor.Client.Services;

public class AuthService
{
    private readonly HttpClient _httpClient;

    public AuthService(HttpClient httpClient, IJSRuntime jsRuntime)
    {
        _httpClient = httpClient;
    }

    // GylleneDroppen.Blazor.Client/Services/AuthService.cs

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

            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Login error: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> LogoutAsync()
    {
        var response = await _httpClient.PostAsync("api/auth/logout", null);
        return response.IsSuccessStatusCode;
    }

    public async Task<CurrentUserResponse?> GetCurrentUserAsync()
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<CurrentUserResponse>("api/auth/current-user");
        }
        catch
        {
            return null;
        }
    }
}