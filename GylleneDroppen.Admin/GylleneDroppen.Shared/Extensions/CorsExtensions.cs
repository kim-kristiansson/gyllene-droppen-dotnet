using GylleneDroppen.Api.Options;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace GylleneDroppen.Shared.Extensions;

public static class CorsExtensions
{
    private const string CorsPolicyName = "AllowFrontend";

    public static void AddCustomCors(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<GlobalOptions>(configuration.GetSection("GlobalOptions"));

        services.AddCors(options =>
        {
            options.AddPolicy(CorsPolicyName, policy =>
            {
                var globalOptions = services.BuildServiceProvider().GetRequiredService<IOptions<GlobalOptions>>();

                policy.WithOrigins(globalOptions.Value.FrontendBaseUrl, globalOptions.Value.AdminBaseUrl)
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