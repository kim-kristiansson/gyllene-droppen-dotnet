using System.Net;
using System.Net.Mail;
using GylleneDroppen.Infrastructure.Cofiguration.Settings;

namespace GylleneDroppen.Infrastructure.Email;

public static class SmtpClientFactory
{
    public static SmtpClient Create(SmtpSettings smtpOptions)
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