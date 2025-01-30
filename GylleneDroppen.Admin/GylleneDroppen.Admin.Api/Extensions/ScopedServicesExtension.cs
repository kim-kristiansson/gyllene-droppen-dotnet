using System.Reflection;
using GylleneDroppen.Admin.Api.Services;
using GylleneDroppen.Admin.Api.Services.Interfaces;

namespace GylleneDroppen.Admin.Api.Extensions;

public static class ScopedServicesExtension
{
    public static void AddScopedServices(this IServiceCollection services)
    {
        var serviceType = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(t => t is {IsClass: true, IsAbstract: false} && 
                        t.GetInterfaces().Any(i => i.Name.EndsWith("Service")))
            .ToList();

        foreach (var implementationType in serviceType)
        {
            var interfaceType = implementationType.GetInterfaces()
                .FirstOrDefault(i => i.Name == $"I{implementationType.Name}");

            if (interfaceType is not null)
            {
                services.AddScoped(interfaceType, implementationType);
            }
        }
    }
}