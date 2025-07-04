using GylleneDroppen.Application.Dtos.User;

namespace GylleneDroppen.Application.Interfaces.Services;

public interface IUserManagementService
{
    Task<List<UserResponseDto>> GetAllUsersAsync();
    Task<UserResponseDto?> GetUserByIdAsync(string userId);
    Task<bool> PromoteToAdminAsync(string userId, string currentAdminId);
    Task<bool> DemoteFromAdminAsync(string userId, string currentAdminId);
    Task<bool> IsUserAdminAsync(string userId);
    Task<List<UserResponseDto>> GetAdminUsersAsync();
}