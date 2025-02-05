using System.Reflection;

namespace GylleneDroppen.Admin.Api.Extensions;

public static class ConfigureOptionsExtensions
{
    public static void AddConfigureOptions(this IServiceCollection services, IConfiguration configuration)
    {
        var configureMethod = typeof(OptionsConfigurationServiceCollectionExtensions)
            .GetMethods()
            .First(m => m.Name == nameof(OptionsConfigurationServiceCollectionExtensions.Configure)
                        && m.GetGenericArguments().Length == 1
                        && m.GetParameters().Length == 2
                        && m.GetParameters()[1].ParameterType == typeof(IConfiguration));

        var optionTypes = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(t => t is { IsClass: true, IsAbstract: false } &&
                        t.GetProperties().Any(p => p.SetMethod is not null)) 
            .ToList();

        foreach (var configType in optionTypes)
        {
            var sectionName = configType.Name; 
            var genericMethod = configureMethod.MakeGenericMethod(configType);
            genericMethod.Invoke(null, [services, configuration.GetSection(sectionName)]);
        }
    }
}