using GylleneDroppen.Infrastructure.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GylleneDroppen.Infrastructure.Configuration;

public static class ApplicationConfigurationExtension
{
    public static IServiceCollection AddApplicationConfiguration(this IServiceCollection services,
        IConfiguration configuration)
    {
        // Bind strongly-typed settings from configuration
        services.Configure<DatabaseSettings>(configuration.GetRequiredSection("DatabaseSettings"));
        services.Configure<RedisSettings>(configuration.GetRequiredSection("RedisSettings"));
        services.Configure<SmtpSettings>(configuration.GetRequiredSection("SmtpSettings"));
        services.Configure<GlobalSettings>(configuration.GetRequiredSection("GlobalSettings"));
        services.Configure<AdminSettings>(configuration.GetRequiredSection("AdminSettings"));

        // Log database connection preview
        var databaseSettings = configuration
            .GetSection("DatabaseSettings")
            .Get<DatabaseSettings>();

        if (databaseSettings?.ConnectionString is { Length: > 0 } connectionString)
        {
            var preview = connectionString.Length > 20
                ? $"{connectionString[..20]}..."
                : connectionString;

            Console.WriteLine($"[Config] Using database connection string: {preview}");
        }
        else
        {
            Console.WriteLine("⚠️ WARNING: Missing or empty DatabaseSettings:ConnectionString in configuration.");
        }

        return services;
    }
}