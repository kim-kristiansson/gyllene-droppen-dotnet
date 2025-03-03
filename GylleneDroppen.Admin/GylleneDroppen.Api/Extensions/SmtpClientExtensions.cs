using System.Net.Mail;
using GylleneDroppen.Api.Options;
using GylleneDroppen.Api.Utilities;
using Microsoft.Extensions.Options;

namespace GylleneDroppen.Api.Extensions;

public static class SmtpClientExtensions
{
    public static void AddSmtpClient(this IServiceCollection services)
    {
        services.AddSingleton<SmtpClient>(serviceProvider =>
        {
            var smtpOptions = serviceProvider.GetRequiredService<IOptions<SmtpOptions>>().Value;
            return SmtpClientFactory.Create(smtpOptions);
        });
    }
}