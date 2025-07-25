using GylleneDroppen.Application.Interfaces.Repositories;
using GylleneDroppen.Application.Interfaces.Services;
using GylleneDroppen.Application.Services;
using GylleneDroppen.Infrastructure.Configuration;
using GylleneDroppen.Infrastructure.Persistence.Repositories;
using GylleneDroppen.Infrastructure.Services;
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
        services.AddScoped<IWhiskyTypeRepository, WhiskyTypeRepository>();
        services.AddScoped<ICountryRepository, CountryRepository>();
        services.AddScoped<IRegionRepository, RegionRepository>();
        services.AddScoped<ITastingHistoryRepository, TastingHistoryRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ITastingEventRepository, TastingEventRepository>();
        services.AddScoped<ITastingEventWhiskyRepository, TastingEventWhiskyRepository>();
        services.AddScoped<ITastingEventParticipantRepository, TastingEventParticipantRepository>();
        services.AddScoped<IMembershipPeriodRepository, MembershipPeriodRepository>();
        services.AddScoped<IUserMembershipRepository, UserMembershipRepository>();
        services.AddScoped<IUserTrialUsageRepository, UserTrialUsageRepository>();
        services.AddScoped<IDepartmentRepository, DepartmentRepository>();
        services.AddScoped<IAddressRepository, AddressRepository>();

        return services;
    }

    private static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();

        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddScoped<IImageService, ImageService>();

        services.AddScoped<IUserManagementService, UserManagementService>();
        services.AddScoped<IWhiskyService, WhiskyService>();
        services.AddScoped<IWhiskyMetadataService, WhiskyMetadataService>();
        services.AddScoped<ITastingHistoryService, TastingHistoryService>();
        services.AddScoped<ITastingEventService, TastingEventService>();
        services.AddScoped<IMembershipService, MembershipService>();
        services.AddScoped<IDepartmentService, DepartmentService>();
        services.AddScoped<IAddressService, AddressService>();

        return services;
    }
}