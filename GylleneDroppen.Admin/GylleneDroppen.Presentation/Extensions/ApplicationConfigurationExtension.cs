using GylleneDroppen.Infrastructure.Settings;

namespace GylleneDroppen.Presentation.Extensions;

public static class ApplicationConfigurationExtension
{
    public static void AddApplicationConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<DatabaseSettings>(configuration.GetSection("DatabaseSettings"));
        services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));
        services.Configure<RedisSettings>(configuration.GetSection("RedisSettings"));
        services.Configure<SmtpSettings>(configuration.GetSection("SmtpSettings"));
        services.Configure<GlobalSettings>(configuration.GetSection("GlobalSettings"));

        var databaseSettings = configuration.GetSection("DatabaseSettings").Get<DatabaseSettings>();
        Console.WriteLine(databaseSettings != null
            ? $"Configuring DatabaseSettings with connection string: {databaseSettings.ConnectionString?[..Math.Min(20, databaseSettings.ConnectionString?.Length ?? 0)]}..."
            : "WARNING: DatabaseSettings section is missing or empty in configuration!");
    }
}