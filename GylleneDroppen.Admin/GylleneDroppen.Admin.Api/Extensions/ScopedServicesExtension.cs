using GylleneDroppen.Admin.Api.Services;
using GylleneDroppen.Admin.Api.Services.Interfaces;

namespace GylleneDroppen.Admin.Api.Extensions;

public static class ScopedServicesExtension
{
    public static void AddScopedServices(this IServiceCollection services)
    {
        var serviceMappings = new Dictionary<Type, Type>
        {
            { typeof(IJwtService), typeof(JwtService) },
        };

        foreach (var mapping in serviceMappings)
        {
            services.AddScoped(mapping.Key, mapping.Value);
        }
    }
}