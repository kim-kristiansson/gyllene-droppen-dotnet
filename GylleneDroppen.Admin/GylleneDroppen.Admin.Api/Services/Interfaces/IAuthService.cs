using GylleneDroppen.Admin.Api.Dtos;
using GylleneDroppen.Admin.Api.Utilities;

namespace GylleneDroppen.Admin.Api.Services.Interfaces;

public interface IAuthService
{
    Task<ServiceResponse<LoginResponse>> LoginAsync(LoginRequest request);
    Task<ServiceResponse<MessageResponse>> RegisterAsync(RegisterRequest request);
    Task<ServiceResponse<MessageResponse>> LogoutAsync(string accessToken, LogoutRequest request);
    Task<ServiceResponse<RefreshTokenResponse>> RefreshTokenAsync(RefreshTokenRequest request);
}