using GylleneDroppen.Shared.Dtos.Auth;
using GylleneDroppen.Shared.Dtos.Generic;
using GylleneDroppen.Shared.Utils;

namespace GylleneDroppen.Core.Interfaces.Services;

public interface IAuthService
{
    Task<ServiceResponse<LoginResponse>> LoginAsync(LoginRequest request);
    Task<ServiceResponse<MessageResponse>> RegisterAsync(RegisterRequest request);
    Task<ServiceResponse<MessageResponse>> LogoutAsync();
    Task<ServiceResponse<MessageResponse>> RefreshTokenAsync();
    Task<ServiceResponse<MessageResponse>> ConfirmEmailAsync(ConfirmEmailRequest request);
    Task<ServiceResponse<MessageResponse>> RequestPasswordResetAsync(PasswordResetRequest request);
    Task<ServiceResponse<MessageResponse>> ResetPasswordAsync(ResetPasswordRequest request);
}