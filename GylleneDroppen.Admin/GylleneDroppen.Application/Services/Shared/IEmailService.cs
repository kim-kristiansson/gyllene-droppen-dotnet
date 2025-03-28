namespace GylleneDroppen.Application.Services.Shared;

public interface IEmailService
{
    Task<ServiceResponse<MessageResponse>> SendEmailConfirmationCodeAsync(string email, string confirmationCode);
    Task<ServiceResponse<MessageResponse>> SendPasswordResetEmailAsync(string email, string resetToken);
}