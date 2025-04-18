using GylleneDroppen.Application.Common.Results;
using GylleneDroppen.Application.Dtos.Common;

namespace GylleneDroppen.Application.Services.Shared;

public interface IEmailService
{
    Task<Result<MessageResponse>> SendEmailConfirmationCodeAsync(string email, string confirmationCode);
    Task<Result<MessageResponse>> SendPasswordResetEmailAsync(string email, string resetToken);
}