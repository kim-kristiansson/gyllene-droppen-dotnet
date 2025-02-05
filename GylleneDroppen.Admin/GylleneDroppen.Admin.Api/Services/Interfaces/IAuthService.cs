using GylleneDroppen.Admin.Api.Utilities;

namespace GylleneDroppen.Admin.Api.Services.Interfaces;

public interface IAuthService
{
    Task<ServiceResponse<string>> LoginAsync(string email, string password);
    Task<ServiceResponse<bool>> RegisterAsync(string email, string password);
}