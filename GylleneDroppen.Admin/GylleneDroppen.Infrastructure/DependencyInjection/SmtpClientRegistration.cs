using GylleneDroppen.Infrastructure.Email;
using GylleneDroppen.Infrastructure.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GylleneDroppen.Infrastructure.DependencyInjection;

public static class SmtpClientRegistration
{
    public static IServiceCollection AddSmtpClient(this IServiceCollection services, IConfiguration configuration)
    {
        var smtpSettings = configuration.GetSection("SmtpSettings").Get<SmtpSettings>();
        if (smtpSettings == null)
            throw new InvalidOperationException("Missing or invalid SmtpSettings in configuration.");

        services.AddSingleton(_ => SmtpClientFactory.Create(smtpSettings));
        return services;
    }
}