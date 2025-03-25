using GylleneDroppen.Core.Common;
using GylleneDroppen.Core.Dtos.Auth;
using GylleneDroppen.Core.Dtos.Generic;

namespace GylleneDroppen.Core.Interfaces.Services;

public interface IAuthService
{
    Task<Result<LoginResponse>> LoginAsync(LoginRequest request);
    Task<Result<MessageResponse>> RegisterAsync(RegisterRequest request);
    Task<Result<MessageResponse>> LogoutAsync();
    Task<Result<MessageResponse>> RefreshTokenAsync();
    Task<Result<MessageResponse>> ConfirmEmailAsync(ConfirmEmailRequest request);
    Task<Result<MessageResponse>> RequestPasswordResetAsync(PasswordResetRequest request);
    Task<Result<MessageResponse>> ResetPasswordAsync(ResetPasswordRequest request);
}