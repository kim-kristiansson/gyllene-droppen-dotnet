using System.Net.Mail;
using GylleneDroppen.Application.Common.Results;
using GylleneDroppen.Application.Dtos.Common;
using GylleneDroppen.Application.Services.Shared;
using GylleneDroppen.Infrastructure.Settings;
using Microsoft.Extensions.Options;

namespace GylleneDroppen.Infrastructure.Email;

public class EmailService(SmtpClient smtpClient, IOptions<GlobalSettings> globalOptions) : IEmailService
{
    public async Task<Result<MessageResponse>> SendEmailConfirmationCodeAsync(string email,
        string confirmationCode)
    {
        var confirmationLink =
            $"{globalOptions.Value.FrontendBaseUrl}/verify-email?email={email}&code={confirmationCode}";

        var message = new MailMessage
        {
            From = new MailAddress("no-reply@gyllenedroppen.se"),
            Subject = "Gyllene Droppen - Verifieringskod",
            Body = $"""
                    Använd denna kod för att verifiera din e-post: {confirmationCode}
                    Eller <a href='{confirmationLink}'>klicka här</a> för att verifiera direkt.
                    """,
            IsBodyHtml = true
        };

        message.To.Add(new MailAddress(email));
        await smtpClient.SendMailAsync(message);

        return Result<MessageResponse>.Success(
            new MessageResponse("Email verification code sent successfully"));
    }

    public async Task<Result<MessageResponse>> SendPasswordResetEmailAsync(string email, string resetToken)
    {
        var confirmationLink = $"{globalOptions.Value.FrontendBaseUrl}/reset-password?email={email}&code={resetToken}";

        var message = new MailMessage
        {
            From = new MailAddress("no-reply@gyllenedroppen.se"),
            Subject = "Gyllene Droppen - Återställ Lösenord",
            Body = $"""
                    Använd denna kod för att verifiera din e-post: {resetToken}
                    Eller <a href='{confirmationLink}'>klicka här</a> för att verifiera direkt.
                    """,
            IsBodyHtml = true
        };

        message.To.Add(new MailAddress(email));
        await smtpClient.SendMailAsync(message);

        return Result<MessageResponse>.Success(
            new MessageResponse("Email verification code sent successfully"));
    }
}