using GylleneDroppen.Api.Dtos;
using GylleneDroppen.Api.Utilities;

namespace GylleneDroppen.Api.Services.Interfaces;

public interface IEmailService
{
    Task<ServiceResponse<MessageResponse>> SendEmailConfirmationCodeAsync(string email, string confirmationCode);
    Task<ServiceResponse<MessageResponse>> SendPasswordResetEmailAsync(string email, string resetToken);
}