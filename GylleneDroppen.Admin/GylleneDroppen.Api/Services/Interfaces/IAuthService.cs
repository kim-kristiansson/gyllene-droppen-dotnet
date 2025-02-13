using GylleneDroppen.Api.Dtos;
using GylleneDroppen.Api.Utilities;

namespace GylleneDroppen.Api.Services.Interfaces;

public interface IAuthService
{
    Task<ServiceResponse<LoginResponse>> LoginAsync(LoginRequest request);
    Task<ServiceResponse<MessageResponse>> RegisterAsync(RegisterRequest request);
    Task<ServiceResponse<MessageResponse>> LogoutAsync(LogoutRequest request, string accessToken);
    Task<ServiceResponse<RefreshTokenResponse>> RefreshTokenAsync(RefreshTokenRequest request, string accessToken);
    Task<ServiceResponse<MessageResponse>> ConfirmEmailAsync(ConfirmEmailRequest request);
}