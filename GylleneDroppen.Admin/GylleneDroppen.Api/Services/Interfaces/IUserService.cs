using GylleneDroppen.Api.Dtos;
using GylleneDroppen.Api.Utilities;

namespace GylleneDroppen.Api.Services.Interfaces;

public interface IUserService
{
    Task<ServiceResponse<MessageResponse>> CreateUserAsync(string email, string password);
}