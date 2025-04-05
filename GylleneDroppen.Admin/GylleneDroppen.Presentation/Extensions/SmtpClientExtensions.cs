using System.Net.Mail;
using GylleneDroppen.Infrastructure.Email;
using GylleneDroppen.Infrastructure.Settings;
using Microsoft.Extensions.Options;

namespace GylleneDroppen.Presentation.Extensions;

public static class SmtpClientExtensions
{
    public static void AddSmtpClient(this IServiceCollection services)
    {
        services.AddSingleton<SmtpClient>(serviceProvider =>
        {
            var smtpSettings = serviceProvider.GetRequiredService<IOptions<SmtpSettings>>().Value;
            return SmtpClientFactory.Create(smtpSettings);
        });
    }
}