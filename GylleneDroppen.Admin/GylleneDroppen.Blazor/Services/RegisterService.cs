using System.Net;
using System.Net.Http.Json;
using GylleneDroppen.Application.Dtos.Auth;
using GylleneDroppen.Application.Dtos.Common;
using Microsoft.AspNetCore.Components;

namespace GylleneDroppen.Blazor.Services;

public class RegisterService(
    HttpClient httpClient,
    NavigationManager navigationManager,
    ILogger<RegisterService> logger)
    : BaseApiService(httpClient, navigationManager, logger)
{
    public async Task<RegisterResult> RegisterAsync(RegisterRequest request)
    {
        try
        {
            await EnsureInitializedAsync();

            Logger.LogInformation("Attempting registration for user: {Email}", request.Email);
            var response = await HttpClient.PostAsJsonAsync("api/auth/register", request);

            if (response.IsSuccessStatusCode)
            {
                Logger.LogInformation("Registration successful for user: {Email}", request.Email);
                return new RegisterResult
                {
                    Success = true,
                    Message = "Registrering lyckades! Kontrollera din e-post för verifieringsinstruktioner."
                };
            }

            string errorMessage;

            if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                try
                {
                    var errorResponse = await response.Content.ReadFromJsonAsync<MessageResponse>();
                    errorMessage = errorResponse?.Message ?? "Ett fel uppstod vid registreringen.";
                }
                catch
                {
                    errorMessage = "Ett fel uppstod vid registreringen.";
                }

                Logger.LogWarning("Registration failed for user: {Email}, Error: {Error}",
                    request.Email, errorMessage);

                return new RegisterResult
                {
                    Success = false,
                    Message = errorMessage
                };
            }

            errorMessage = "Ett tekniskt fel uppstod. Vänligen försök igen senare.";
            Logger.LogWarning("Registration failed for user: {Email}, Status: {StatusCode}",
                request.Email, response.StatusCode);

            return new RegisterResult
            {
                Success = false,
                Message = errorMessage
            };
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Exception during registration for user: {Email}", request.Email);
            return new RegisterResult
            {
                Success = false,
                Message = "Ett tekniskt fel inträffade. Försök igen senare."
            };
        }
    }
}

public class RegisterResult
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
}