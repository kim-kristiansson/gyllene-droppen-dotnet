using GylleneDroppen.Application.Interfaces.Repositories;
using GylleneDroppen.Application.Interfaces.Services;
using GylleneDroppen.Application.Services;
using GylleneDroppen.Infrastructure.Configuration;
using GylleneDroppen.Infrastructure.Persistence.Repositories;
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
            .AddRepositories()
            .AddApplicationServices();

        return services;
    }

    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IWhiskyRepository, WhiskyRepository>();
        services.AddScoped<ITastingHistoryRepository, TastingHistoryRepository>();
        services.AddScoped<IUserRepository, UserRepository>();

        return services;
    }

    private static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IUserManagementService, UserManagementService>();
        services.AddScoped<IWhiskyService, WhiskyService>();
        services.AddScoped<ITastingHistoryService, TastingHistoryService>();

        return services;
    }
}