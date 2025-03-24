using System.Net;
using System.Net.Mail;
using GylleneDroppen.Api.Options;

namespace GylleneDroppen.Infrastructure.Utilities;

public static class SmtpClientFactory
{
    public static SmtpClient Create(SmtpOptions smtpOptions)
    {
        return new SmtpClient
        {
            Host = smtpOptions.Host,
            Port = smtpOptions.Port,
            Credentials = new NetworkCredential(
                smtpOptions.Username,
                smtpOptions.Password
            ),
            EnableSsl = true
        };
    }
}