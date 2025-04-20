using GylleneDroppen.Application.Common.Results;
using GylleneDroppen.Application.Dtos.Admin.User;

namespace GylleneDroppen.Application.Interfaces.Admin.Services;

public interface IAdminUserService
{
    Task<Result<List<AdminUserDetailResponse>>> GetAllUsersAsync();
    Task<Result<AdminUserDetailResponse>> GetUserByIdAsync(Guid userId);
}