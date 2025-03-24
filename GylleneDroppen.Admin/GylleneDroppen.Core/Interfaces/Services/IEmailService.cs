using GylleneDroppen.Shared.Dtos.Generic;
using GylleneDroppen.Shared.Utils;

namespace GylleneDroppen.Core.Interfaces.Services;

public interface IEmailService
{
    Task<ServiceResponse<MessageResponse>> SendEmailConfirmationCodeAsync(string email, string confirmationCode);
    Task<ServiceResponse<MessageResponse>> SendPasswordResetEmailAsync(string email, string resetToken);
}