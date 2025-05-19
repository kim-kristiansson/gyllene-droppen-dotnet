using GylleneDroppen.Application.Interfaces.Services;
using GylleneDroppen.Application.Interfaces.Utilities;
using GylleneDroppen.Application.Services;
using GylleneDroppen.Infrastructure.Configuration;
using GylleneDroppen.Infrastructure.Email;
using GylleneDroppen.Infrastructure.Utilities;
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
            .AddScoped<IEmailService, EmailService>()
            .AddScoped<IUrlGenerator, UrlGenerator>()
            .AddScoped<IAuthService, AuthService>();
        return services;
    }
}