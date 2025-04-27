using System.Net.Http.Json;

namespace GylleneDroppen.Admin.Blazor.Services;

public class AppConfigService(HttpClient httpClient, ILogger<AppConfigService> logger)
{
    private AppSettings? _settings;

    private async Task<AppSettings> GetSettingsAsync()
    {
        if (_settings != null)
            return _settings;

        try
        {
            _settings = await httpClient.GetFromJsonAsync<AppSettings>("appsettings.json");
            return _settings ?? new AppSettings();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to load application settings");
            return new AppSettings();
        }
    }

    public async Task<string> GetApiBaseUrlAsync()
    {
        var settings = await GetSettingsAsync();

        return settings.EnvironmentSettings.Environment.Equals("Development", StringComparison.OrdinalIgnoreCase)
            ? settings.ApiSettings.LocalUrl
            : settings.ApiSettings.BaseUrl;
    }
}

public class AppSettings
{
    public ApiSettings ApiSettings { get; set; } = new();
    public EnvironmentSettings EnvironmentSettings { get; set; } = new();
}

public class ApiSettings
{
    public string BaseUrl { get; set; } = string.Empty;
    public string LocalUrl { get; set; } = string.Empty;
    public string Environment { get; set; } = string.Empty;
}

public class EnvironmentSettings
{
    public string Environment { get; set; } = "Production";
}