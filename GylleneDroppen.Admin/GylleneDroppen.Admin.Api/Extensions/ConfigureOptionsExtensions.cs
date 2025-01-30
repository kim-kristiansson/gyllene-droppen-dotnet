using GylleneDroppen.Admin.Api.Options;

namespace GylleneDroppen.Admin.Api.Extensions;

public static class ConfigureOptionsExtensions
{
    public static IServiceCollection AddAppConfigurations(this IServiceCollection services, IConfiguration configuration)
    {
        var configMappings = new Dictionary<Type, string>
        {
            { typeof(JwtOptions), "JwtOptions" },
        };

        foreach (var (configType, sectionName) in configMappings)
        {
            var method = typeof(OptionsConfigurationServiceCollectionExtensions)
                .GetMethod("Configure", [typeof(IServiceCollection), typeof(IConfigurationSection)])!
                .MakeGenericMethod(configType);

            method.Invoke(null, [services, configuration.GetSection(sectionName)]);
        }

        return services;
    }
}