using GylleneDroppen.Core.Common;
using GylleneDroppen.Core.Dtos.Generic;

namespace GylleneDroppen.Core.Interfaces.Services;

public interface IEmailService
{
    Task<Result<MessageResponse>> SendEmailConfirmationCodeAsync(string email, string confirmationCode);
    Task<Result<MessageResponse>> SendPasswordResetEmailAsync(string email, string resetToken);
}