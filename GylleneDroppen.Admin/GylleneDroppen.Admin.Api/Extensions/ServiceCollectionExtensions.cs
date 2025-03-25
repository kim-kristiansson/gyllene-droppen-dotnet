using GylleneDroppen.Admin.Api.Interfaces.Mappers;
using GylleneDroppen.Admin.Api.Interfaces.Repositories;
using GylleneDroppen.Admin.Api.Interfaces.Services;
using GylleneDroppen.Admin.Api.Mappers;
using GylleneDroppen.Admin.Api.Repositories;
using GylleneDroppen.Admin.Api.Services;

namespace GylleneDroppen.Admin.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddApiServices(this IServiceCollection services)
    {
        // Services
        services.AddScoped<IWhiskyTastingService, WhiskyTastingService>();
        // services.AddScoped<IAuthService, AuthService>();

        // Repositories
        services.AddScoped<IWhiskyTastingRepository, WhiskyTastingRepository>();

        // Mappers
        services.AddScoped<IWhiskyTastingMapper, WhiskyTastingMapper>();
    }
}