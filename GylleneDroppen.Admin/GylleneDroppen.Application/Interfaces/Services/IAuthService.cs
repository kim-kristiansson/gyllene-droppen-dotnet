using GylleneDroppen.Application.Common.Results;
using GylleneDroppen.Application.Dtos.Common;
using GylleneDroppen.Application.Dtos.Email;
using GylleneDroppen.Application.Dtos.Shared.Auth;

namespace GylleneDroppen.Application.Interfaces.Services;

public interface IAuthService
{
    Task<Result<MessageResponse>> LoginAsync(LoginRequest request);
    Task<Result<MessageResponse>> RegisterAsync(RegisterRequest request);
    Task<Result<MessageResponse>> LogoutAsync();
    Task<Result<MessageResponse>> RefreshTokenAsync();
    Task<Result<MessageResponse>> ConfirmEmailAsync(ConfirmEmailRequest request);
    Task<Result<MessageResponse>> RequestPasswordResetAsync(PasswordResetRequest request);
    Task<Result<MessageResponse>> ResetPasswordAsync(ResetPasswordRequest request);
    Task<Result<CurrentUserResponse>> GetCurrentUserAsync();
    Task<Result<MessageResponse>> DeleteAccountAsync(Guid userId, string password);
}