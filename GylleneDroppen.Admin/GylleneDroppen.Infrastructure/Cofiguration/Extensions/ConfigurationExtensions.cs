using System.Reflection;
using GylleneDroppen.Infrastructure.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GylleneDroppen.Infrastructure.Cofiguration.Extensions;

public static class ConfigurationExtensions
{
    public static IServiceCollection AddApplicationConfiguration(
        this IServiceCollection services,
        IConfiguration configuration,
        string configurationNameSuffix = "Settings")
    {
        services.Configure<DatabaseSettings>(configuration.GetSection(nameof(DatabaseSettings)));
        services.Configure<GlobalSettings>(configuration.GetSection(nameof(GlobalSettings)));
        services.Configure<JwtSettings>(configuration.GetSection(nameof(JwtSettings)));
        services.Configure<RedisSettings>(configuration.GetSection(nameof(RedisSettings)));
        services.Configure<SmtpSettings>(configuration.GetSection(nameof(SmtpSettings)));

        RegisterAdditionalConfiguration(services, configuration, Assembly.GetCallingAssembly(),
            configurationNameSuffix);

        return services;
    }

    public static IServiceCollection AddConfigurationFromAssembly(
        this IServiceCollection services,
        IConfiguration configuration,
        Assembly assembly,
        string configurationNameSuffix = "Settings")
    {
        RegisterAdditionalConfiguration(services, configuration, assembly, configurationNameSuffix);
        return services;
    }

    private static void RegisterAdditionalConfiguration(
        IServiceCollection services,
        IConfiguration configuration,
        Assembly assembly,
        string configurationNameSuffix)
    {
        var configureMethod = typeof(OptionsConfigurationServiceCollectionExtensions)
            .GetMethods()
            .First(m => m.Name == nameof(OptionsConfigurationServiceCollectionExtensions.Configure)
                        && m.GetGenericArguments().Length == 1
                        && m.GetParameters().Length == 2
                        && m.GetParameters()[1].ParameterType == typeof(IConfiguration));

        var configTypes = assembly
            .GetTypes()
            .Where(t => t.Name.EndsWith(configurationNameSuffix) &&
                        t is { IsClass: true, IsAbstract: false } &&
                        t.GetProperties().Any(p => p.SetMethod is not null))
            .ToList();

        foreach (var configType in configTypes)
        {
            var sectionName = configType.Name;
            var genericMethod = configureMethod.MakeGenericMethod(configType);
            genericMethod.Invoke(null, [services, configuration.GetSection(sectionName)]);
        }
    }
}