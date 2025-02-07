using GylleneDroppen.Admin.Api.Dtos;
using GylleneDroppen.Admin.Api.Utilities;

namespace GylleneDroppen.Admin.Api.Services.Interfaces;

public interface IAuthService
{
    Task<ServiceResponse<LoginResponse>> LoginAsync(LoginRequest loginRequest);
    Task<ServiceResponse<MessageResponse>> RegisterAsync(RegisterRequest registerRequest);
    Task<ServiceResponse<MessageResponse>> LogoutAsync(string token);
}