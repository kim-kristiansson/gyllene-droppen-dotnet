using GylleneDroppen.Application.Interfaces.Services;
using GylleneDroppen.Application.Services;
using GylleneDroppen.Infrastructure.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GylleneDroppen.Infrastructure.DependencyInjection;

public static class InfrastructionCollectionExtension
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        services
            .AddApplicationConfiguration(config)
            .AddSmtpClient(config)
            .AddDatabase()
            .AddScoped<IUserManagementService, UserManagementService>();
        return services;
    }
}