using GylleneDroppen.Api.Dtos;
using GylleneDroppen.Api.Utilities;

namespace GylleneDroppen.Api.Services.Interfaces;

public interface IEmailService
{
    Task<ServiceResponse<MessageResponse>> SendEmailVerificationCodeAsync(string email, string confirmationCode, string confirmationLink);
}