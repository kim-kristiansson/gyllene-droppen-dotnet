using GylleneDroppen.Infrastructure.Settings;
using Microsoft.Extensions.Options;

namespace GylleneDroppen.Presentation.Extensions;

public static class CorsExtensions
{
    private const string CorsPolicyName = "AllowFrontend";

    public static void AddCustomCors(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<GlobalSettings>(configuration.GetSection("GlobalSettings"));

        services.AddCors(options =>
        {
            options.AddPolicy(CorsPolicyName, policy =>
            {
                var globalSettings = services.BuildServiceProvider().GetRequiredService<IOptions<GlobalSettings>>();

                policy.WithOrigins(globalSettings.Value.FrontendBaseUrl, globalSettings.Value.AdminBaseUrl)
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
            });
        });
    }

    public static void UseCustomCors(this IApplicationBuilder app)
    {
        app.UseCors(CorsPolicyName);
    }
}