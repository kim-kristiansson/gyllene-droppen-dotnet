using GylleneDroppen.Admin.Api.Interfaces.Repositories;
using GylleneDroppen.Admin.Api.Interfaces.Services;
using GylleneDroppen.Admin.Api.Repositories;
using GylleneDroppen.Admin.Api.Services;
using GylleneDroppen.Core.Interfaces.Services;
using GylleneDroppen.Infrastructure.Services;

namespace GylleneDroppen.Admin.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddApiServices(this IServiceCollection services)
    {
        // Services
        services.AddScoped<IWhiskyTastingService, WhiskyTastingService>();
        services.AddScoped<IAuthService, AuthService>();

        // Repositories
        services.AddScoped<IWhiskyTastingRepository, WhiskyTastingRepository>();
    }
}