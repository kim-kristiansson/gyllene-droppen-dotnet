using System.Net.Http.Json;
using System.Web;
using Microsoft.AspNetCore.Components;

namespace GylleneDroppen.Blazor.Services;

public class BaseApiService
{
    protected readonly HttpClient HttpClient;
    protected readonly ILogger Logger;
    protected readonly NavigationManager NavigationManager;
    protected string ApiBaseUrl = string.Empty;

    protected bool IsInitialized;

    protected BaseApiService(
        HttpClient httpClient,
        NavigationManager navigationManager,
        ILogger logger)
    {
        HttpClient = httpClient;
        NavigationManager = navigationManager;
        Logger = logger;
    }

    protected async Task EnsureInitializedAsync()
    {
        if (IsInitialized)
            return;

        try
        {
            var response = await HttpClient.GetFromJsonAsync<AppSettings>("AppSettings.json");
            if (response != null)
            {
                ApiBaseUrl = response.ApiSettings.Environment.Equals("Development", StringComparison.OrdinalIgnoreCase)
                    ? response.ApiSettings.LocalUrl
                    : response.ApiSettings.BaseUrl;

                HttpClient.BaseAddress = new Uri(ApiBaseUrl);
                IsInitialized = true;

                Logger.LogInformation("API service initialized with base URL: {BaseUrl}", ApiBaseUrl);
            }
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Failed to initialize API service");
            throw;
        }
    }

    protected Dictionary<string, string> ExtractQueryParameters()
    {
        try
        {
            var uri = NavigationManager.ToAbsoluteUri(NavigationManager.Uri);
            var queryParams = HttpUtility.ParseQueryString(uri.Query);

            var result = new Dictionary<string, string>();
            foreach (var key in queryParams.AllKeys)
                if (key != null)
                    result[key] = queryParams[key] ?? string.Empty;

            return result;
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error extracting query parameters from URL");
            return new Dictionary<string, string>();
        }
    }

    public class AppSettings
    {
        public ApiSettings ApiSettings { get; set; } = new();
    }

    public class ApiSettings
    {
        public string BaseUrl { get; set; } = string.Empty;
        public string LocalUrl { get; set; } = string.Empty;
        public string Environment { get; set; } = "Production";
    }
}