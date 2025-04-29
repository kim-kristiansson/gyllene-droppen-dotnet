using System.Net;
using Microsoft.AspNetCore.Components;

namespace GylleneDroppen.Blazor.Services;

public class AuthenticationHandler(
    TokenRefreshService tokenRefreshService,
    ILogger<AuthenticationHandler> logger,
    NavigationManager navigationManager)
    : DelegatingHandler
{
    private bool _isRefreshing;

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        var response = await base.SendAsync(request, cancellationToken);

        // If we receive a 401 and it's not the refresh endpoint
        if (request.RequestUri != null && (response.StatusCode != HttpStatusCode.Unauthorized ||
                                           request.RequestUri.PathAndQuery.Contains("refresh-token") ||
                                           _isRefreshing)) return response;
        _isRefreshing = true;

        try
        {
            var refreshed = await tokenRefreshService.RefreshTokenIfNeededAsync();

            if (refreshed)
            {
                // The token has been refreshed, now retry the original request
                var newRequest = new HttpRequestMessage(request.Method, request.RequestUri);

                // Copy content if any
                if (request.Content != null) newRequest.Content = request.Content;

                return await base.SendAsync(newRequest, cancellationToken);
            }
            else
            {
                // If we couldn't refresh the token, redirect to login
                navigationManager.NavigateTo("/login", true);
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error in token refresh flow");
        }
        finally
        {
            _isRefreshing = false;
        }

        return response;
    }
}