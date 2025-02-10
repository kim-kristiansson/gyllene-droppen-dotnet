using System.Net.Mail;
using GylleneDroppen.Api.Dtos;
using GylleneDroppen.Api.Services.Interfaces;
using GylleneDroppen.Api.Utilities;

namespace GylleneDroppen.Api.Services;

public class EmailService(SmtpClient smtpClient) : IEmailService
{
    public async Task<ServiceResponse<MessageResponse>> SendEmailVerificationCodeAsync(string email, string verificationCode)
    {
        var message = new MailMessage
        {
            From = new MailAddress("no-reply@gyllenedroppen.se"),
            Subject = "Gyllene Droppen - Verifieringskod",
            Body = $"Din verifieringskod är: {verificationCode}",
            IsBodyHtml = false
        };
        
        message.To.Add(new MailAddress(email));
        await smtpClient.SendMailAsync(message);
        
        return ServiceResponse<MessageResponse>.Success(new MessageResponse("Email verification code sent successfully"));
    }
}