using System.Net.Mail;
using GylleneDroppen.Api.Options;
using GylleneDroppen.Infrastructure.Utilities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace GylleneDroppen.Shared.Web.Extensions;

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