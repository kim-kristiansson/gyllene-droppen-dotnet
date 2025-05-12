using System.Net.Mail;
using GylleneDroppen.Infrastructure.Email;
using GylleneDroppen.Infrastructure.Settings;

namespace GylleneDroppen.Presentation.Extensions;

public static class SmtpClientExtensions
{
    public static void AddSmtpClient(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<SmtpClient>(serviceProvider =>
        {
            var smtpSettings = configuration.GetSection("SmtpSettings").Get<SmtpSettings>();
            return smtpSettings != null ? SmtpClientFactory.Create(smtpSettings) : new SmtpClient();
        });
    }
}