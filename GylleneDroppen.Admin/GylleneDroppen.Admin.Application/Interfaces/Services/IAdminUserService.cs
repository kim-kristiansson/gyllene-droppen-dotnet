using GylleneDroppen.Admin.Application.Dtos.User;
using GylleneDroppen.Application.Common.Results;

namespace GylleneDroppen.Admin.Application.Interfaces.Services;

public interface IAdminUserService
{
    Task<Result<List<AdminUserDetailResponse>>> GetAllUsersAsync();
    Task<Result<AdminUserDetailResponse>> GetUserByIdAsync(Guid userId);
}