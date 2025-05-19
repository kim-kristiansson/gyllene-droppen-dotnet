namespace GylleneDroppen.Application.Interfaces.Services;

public interface IEmailService
{
    Task SendEmailConfirmationAsync(string email, string token);
    Task SendPasswordResetEmailAsync(string email, string token);
}