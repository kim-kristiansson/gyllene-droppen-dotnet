using GylleneDroppen.Application.Common.Results;
using GylleneDroppen.Application.Dtos.Admin.User;
using GylleneDroppen.Application.Interfaces.Admin.Services;
using GylleneDroppen.Application.Interfaces.Shared.Repositories;

namespace GylleneDroppen.Application.Services.Admin;

public class AdminUserService(IUserRepository userRepository) : IAdminUserService
{
    public async Task<Result<List<AdminUserDetailResponse>>> GetAllUsersAsync()
    {
        var users = await userRepository.GetAllAsync();

        var response = users.Select(user => new AdminUserDetailResponse
        {
            Id = user.Id,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Role = user.Role.ToString(),
            CreatedAt = user.CreatedAt
        }).ToList();

        return Result<List<AdminUserDetailResponse>>.Success(response);
    }

    public async Task<Result<AdminUserDetailResponse>> GetUserByIdAsync(Guid userId)
    {
        var user = await userRepository.GetByIdAsync(userId);

        if (user is null)
            return Result<AdminUserDetailResponse>.Failure("User not found.", 404);

        var response = new AdminUserDetailResponse
        {
            Id = user.Id,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Role = user.Role.ToString(),
            CreatedAt = user.CreatedAt
        };

        return Result<AdminUserDetailResponse>.Success(response);
    }
}