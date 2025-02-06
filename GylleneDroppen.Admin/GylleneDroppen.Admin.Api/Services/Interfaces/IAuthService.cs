using GylleneDroppen.Admin.Api.Dtos;
using GylleneDroppen.Admin.Api.Utilities;

namespace GylleneDroppen.Admin.Api.Services.Interfaces;

public interface IAuthService
{
    Task<ServiceResponse<LoginResponse>> LoginAsync(LoginRequest loginRequest);
    Task<ServiceResponse<RegisterResponse>> RegisterAsync(RegisterRequest registerRequest);
}