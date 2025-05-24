using System.Net.Mail;
using GylleneDroppen.Core.Entities;
using Microsoft.AspNetCore.Identity;

namespace GylleneDroppen.Blazor.Email;

internal sealed class RealEmailSender(SmtpClient smtpClient) : IEmailSender<ApplicationUser>
{
    public async Task SendConfirmationLinkAsync(ApplicationUser user, string email, string confirmationLink)
    {
        var message = new MailMessage
        {
            From = new MailAddress("no-reply@gyllenedroppen.se"),
            Subject = "Gyllene Droppen - Verifiera din e-post",
            Body = $@"
                <html>
                <head>
                    <style>
                        body {{ font-family: Helvetica Neue, Arial, sans-serif; line-height: 1.6; color: #333; max-width: 600px; margin: 0 auto; padding: 20px; }}
                        .container {{ background: #f9f9f9; border-radius: 5px; padding: 20px; border: 1px solid #ddd; }}
                        h1 {{ color: #9a7d2e; }}
                        .button {{ display: inline-block; background-color: #9a7d2e; color: white; text-decoration: none; padding: 10px 20px; border-radius: 5px; margin: 20px 0; }}
                        .footer {{ margin-top: 30px; font-size: 12px; color: #999; }}
                    </style>
                </head>
                <body>
                    <div class='container'>
                        <h1>Välkommen till Gyllene Droppen!</h1>
                        <p>Tack för din registrering. För att aktivera ditt konto, vänligen klicka på knappen nedan:</p>
                        <p><a href='{confirmationLink}' class='button'>Verifiera min e-post</a></p>
                        <p>Eller kopiera och klistra in följande URL i din webbläsare:</p>
                        <p>{confirmationLink}</p>
                        <p>Om du inte har registrerat dig på Gyllene Droppen, vänligen ignorera detta meddelande.</p>
                        <div class='footer'>
                            <p>Detta är ett automatiskt meddelande, vänligen svara inte på detta e-postmeddelande.</p>
                            <p>&copy; {DateTime.Now.Year} Gyllene Droppen.</p>
                        </div>
                    </div>
                </body>
                </html>
                ",
            IsBodyHtml = true
        };

        message.To.Add(new MailAddress(email));
        await smtpClient.SendMailAsync(message);
    }

    public async Task SendPasswordResetLinkAsync(ApplicationUser user, string email, string resetLink)
    {
        var message = new MailMessage
        {
            From = new MailAddress("no-reply@gyllenedroppen.se"),
            Subject = "Gyllene Droppen - Återställ lösenord",
            Body = $@"
                <html>
                <head>
                    <style>
                        body {{ font-family: Helvetica Neue, Arial, sans-serif; line-height: 1.6; color: #333; max-width: 600px; margin: 0 auto; padding: 20px; }}
                        .container {{ background: #f9f9f9; border-radius: 5px; padding: 20px; border: 1px solid #ddd; }}
                        h1 {{ color: #9a7d2e; }}
                        .button {{ display: inline-block; background-color: #9a7d2e; color: white; text-decoration: none; padding: 10px 20px; border-radius: 5px; margin: 20px 0; }}
                        .footer {{ margin-top: 30px; font-size: 12px; color: #999; }}
                    </style>
                </head>
                <body>
                    <div class='container'>
                        <h1>Återställ ditt lösenord</h1>
                        <p>Vi har tagit emot en begäran om att återställa lösenordet för ditt konto.</p>
                        <p>För att återställa ditt lösenord, klicka på knappen nedan:</p>
                        <p><a href='{resetLink}' class='button'>Återställ lösenord</a></p>
                        <p>Eller kopiera och klistra in följande URL i din webbläsare:</p>
                        <p>{resetLink}</p>
                        <p>Om du inte har begärt att återställa ditt lösenord, vänligen ignorera detta meddelande.</p>
                        <p>Länken är giltig i 4 timmar.</p>
                        <div class='footer'>
                            <p>Detta är ett automatiskt meddelande, vänligen svara inte på detta e-postmeddelande.</p>
                            <p>&copy; {DateTime.Now.Year} Gyllene Droppen.</p>
                        </div>
                    </div>
                </body>
                </html>
                ",
            IsBodyHtml = true
        };

        message.To.Add(new MailAddress(email));
        await smtpClient.SendMailAsync(message);
    }

    public async Task SendPasswordResetCodeAsync(ApplicationUser user, string email, string resetCode)
    {
        var message = new MailMessage
        {
            From = new MailAddress("no-reply@gyllenedroppen.se"),
            Subject = "Gyllene Droppen - Återställningskod",
            Body = $@"
                <html>
                <head>
                    <style>
                        body {{ font-family: Helvetica Neue, Arial, sans-serif; line-height: 1.6; color: #333; max-width: 600px; margin: 0 auto; padding: 20px; }}
                        .container {{ background: #f9f9f9; border-radius: 5px; padding: 20px; border: 1px solid #ddd; }}
                        h1 {{ color: #9a7d2e; }}
                        .code {{ font-size: 24px; font-weight: bold; background: #e8e8e8; padding: 10px; text-align: center; margin: 20px 0; }}
                        .footer {{ margin-top: 30px; font-size: 12px; color: #999; }}
                    </style>
                </head>
                <body>
                    <div class='container'>
                        <h1>Återställningskod för lösenord</h1>
                        <p>Använd följande kod för att återställa ditt lösenord:</p>
                        <div class='code'>{resetCode}</div>
                        <p>Koden är giltig i 4 timmar.</p>
                        <p>Om du inte har begärt att återställa ditt lösenord, vänligen ignorera detta meddelande.</p>
                        <div class='footer'>
                            <p>Detta är ett automatiskt meddelande, vänligen svara inte på detta e-postmeddelande.</p>
                            <p>&copy; {DateTime.Now.Year} Gyllene Droppen.</p>
                        </div>
                    </div>
                </body>
                </html>
                ",
            IsBodyHtml = true
        };

        message.To.Add(new MailAddress(email));
        await smtpClient.SendMailAsync(message);
    }
}