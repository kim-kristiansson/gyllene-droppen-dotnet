using System.Net;
using System.Net.Http.Json;
using GylleneDroppen.Application.Dtos.Common;
using GylleneDroppen.Application.Dtos.Email;
using Microsoft.AspNetCore.Components;

namespace GylleneDroppen.Blazor.Services;

public class VerificationService(
    HttpClient httpClient,
    NavigationManager navigationManager,
    ILogger<VerificationService> logger)
    : BaseApiService(httpClient, navigationManager, logger)
{
    public async Task<VerificationResult> VerifyEmailAsync(ConfirmEmailRequest request)
    {
        try
        {
            await EnsureInitializedAsync();

            Logger.LogInformation("Attempting to verify email: {Email} with code: {Code}", request.Email,
                request.ConfirmationCode);

            var response = await HttpClient.PostAsJsonAsync("api/auth/confirm-email", request);

            if (response.IsSuccessStatusCode)
            {
                Logger.LogInformation("Email verification successful");

                return new VerificationResult
                {
                    Success = true,
                    Message = "Din e-postadress har verifierats! Du kan nu logga in på ditt konto."
                };
            }

            if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                var errorResponse = await response.Content.ReadFromJsonAsync<MessageResponse>();
                var errorMessage = errorResponse?.Message ??
                                   "Verifieringen misslyckades. Försök igen eller kontakta support.";

                Logger.LogWarning("Email verification failed for: {Email}, Error: {ErrorMessage}", request.Email,
                    errorMessage);

                return new VerificationResult
                {
                    Success = false,
                    Message = errorMessage
                };
            }

            Logger.LogWarning("Email verification failed for: {Email}, Status: {StatusCode}", request.Email,
                response.StatusCode);

            return new VerificationResult
            {
                Success = false,
                Message = "Ett fel uppstod vid verifiering. Försök igen senare."
            };
        }
        catch (Exception e)
        {
            Logger.LogError(e, "Exception during email verification for: {Email}", request.Email);

            return new VerificationResult
            {
                Success = false,
                Message = "Ett tekniskt fel uppstod. Vänligen försök igen senare."
            };
        }
    }

    public (string Email, string Code) ExtractVerificationParameters()
    {
        var parameters = ExtractQueryParameters();

        return (
            parameters.TryGetValue("email", out var email) ? email : string.Empty,
            parameters.TryGetValue("code", out var code) ? code : string.Empty
        );
    }

    public class VerificationResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}