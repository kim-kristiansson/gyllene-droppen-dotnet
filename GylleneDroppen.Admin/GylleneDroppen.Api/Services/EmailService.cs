using System.Net.Mail;
using GylleneDroppen.Api.Dtos;
using GylleneDroppen.Api.Services.Interfaces;
using GylleneDroppen.Api.Utilities;

namespace GylleneDroppen.Api.Services;

public class EmailService(SmtpClient smtpClient) : IEmailService
{
    public async Task<ServiceResponse<MessageResponse>> SendEmailVerificationCodeAsync(string email, string confirmationCode, string confirmationLink)
    {
        var message = new MailMessage
        {
            From = new MailAddress("no-reply@gyllenedroppen.se"),
            Subject = "Gyllene Droppen - Verifieringskod",
            Body = $$"""
                     Använd denna kod för att verifiera din e-post: {{confirmationCode}}
                     Eller <a href='{link}'>klicka här</a> för att verifiera direkt.
                     """,
            IsBodyHtml = true
        };
        
        message.To.Add(new MailAddress(email));
        await smtpClient.SendMailAsync(message);
        
        return ServiceResponse<MessageResponse>.Success(new MessageResponse("Email verification code sent successfully"));
    }
}