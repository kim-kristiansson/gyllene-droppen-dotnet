using GylleneDroppen.Application.Dtos.Auth;

namespace GylleneDroppen.Application.Interfaces.Services;

public interface IAuthService
{
    Task RegisterAsync(RegisterRequestDto dto);
    Task<bool> ValidateCredentialsAsync(string email, string password);
}