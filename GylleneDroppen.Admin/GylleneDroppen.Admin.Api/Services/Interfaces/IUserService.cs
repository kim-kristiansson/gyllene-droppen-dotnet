using GylleneDroppen.Admin.Api.Dtos;
using GylleneDroppen.Admin.Api.Utilities;

namespace GylleneDroppen.Admin.Api.Services.Interfaces;

public interface IUserService
{
    Task<ServiceResponse<MessageResponse>> CreateUserAsync(string email, string password);
}