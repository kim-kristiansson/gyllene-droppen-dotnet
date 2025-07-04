using GylleneDroppen.Application.Dtos.User;
using GylleneDroppen.Application.Interfaces.Services;
using GylleneDroppen.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GylleneDroppen.Application.Services;

public class UserManagementService(
    UserManager<ApplicationUser> userManager,
    RoleManager<IdentityRole> roleManager,
    ILogger<UserManagementService> logger)
    : IUserManagementService
{
    public async Task<List<UserResponseDto>> GetAllUsersAsync()
    {
        var users = await userManager.Users.ToListAsync();
        var userDtos = new List<UserResponseDto>();

        foreach (var user in users)
        {
            var roles = await userManager.GetRolesAsync(user);
            var primaryRole = roles.FirstOrDefault() ?? "User";

            userDtos.Add(new UserResponseDto
            {
                Id = Guid.Parse(user.Id),
                Email = user.Email!,
                FullName = $"{user.FirstName} {user.LastName}",
                Role = primaryRole
            });
        }

        return userDtos.OrderBy(u => u.Email).ToList();
    }

    public async Task<UserResponseDto?> GetUserByIdAsync(string userId)
    {
        var user = await userManager.FindByIdAsync(userId);
        if (user == null)
            return null;

        var roles = await userManager.GetRolesAsync(user);
        var primaryRole = roles.FirstOrDefault() ?? "User";

        return new UserResponseDto
        {
            Id = Guid.Parse(user.Id),
            Email = user.Email!,
            FullName = $"{user.FirstName} {user.LastName}",
            Role = primaryRole
        };
    }

    public async Task<bool> PromoteToAdminAsync(string userId, string currentAdminId)
    {
        logger.LogWarning("🔍 PromoteToAdminAsync called with userId={UserId}, by admin={AdminId}", userId,
            currentAdminId);

        try
        {
            // Verify the current user is an admin
            var currentAdmin = await userManager.FindByIdAsync(currentAdminId);
            if (currentAdmin == null || !await userManager.IsInRoleAsync(currentAdmin, "Admin"))
            {
                logger.LogWarning("Unauthorized attempt to promote user. Current user {CurrentAdminId} is not an admin",
                    currentAdminId);
                return false;
            }

            // Find the user to promote
            var userToPromote = await userManager.FindByIdAsync(userId);
            if (userToPromote == null)
            {
                logger.LogWarning("User {UserId} not found for promotion", userId);
                return false;
            }

            // Check if user is already an admin
            if (await userManager.IsInRoleAsync(userToPromote, "Admin"))
            {
                logger.LogInformation("User {UserId} is already an admin", userId);
                return true;
            }

            // Ensure Admin role exists
            if (!await roleManager.RoleExistsAsync("Admin"))
            {
                await roleManager.CreateAsync(new IdentityRole("Admin"));
                logger.LogInformation("Admin role created");
            }

            // Add user to Admin role
            var result = await userManager.AddToRoleAsync(userToPromote, "Admin");
            if (result.Succeeded)
            {
                logger.LogInformation("User {UserId} ({Email}) promoted to Admin by {CurrentAdminId}",
                    userId, userToPromote.Email, currentAdminId);
                return true;
            }

            logger.LogError("Failed to promote user {UserId} to Admin: {Errors}",
                userId, string.Join(", ", result.Errors.Select(e => e.Description)));
            return false;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error promoting user {UserId} to Admin", userId);
            return false;
        }
    }

    public async Task<bool> DemoteFromAdminAsync(string userId, string currentAdminId)
    {
        try
        {
            // Verify the current user is an admin
            var currentAdmin = await userManager.FindByIdAsync(currentAdminId);
            if (currentAdmin == null || !await userManager.IsInRoleAsync(currentAdmin, "Admin"))
            {
                logger.LogWarning("Unauthorized attempt to demote user. Current user {CurrentAdminId} is not an admin",
                    currentAdminId);
                return false;
            }

            // Prevent self-demotion
            if (userId == currentAdminId)
            {
                logger.LogWarning("Admin {CurrentAdminId} attempted to demote themselves", currentAdminId);
                return false;
            }

            // Find the user to demote
            var userToDemote = await userManager.FindByIdAsync(userId);
            if (userToDemote == null)
            {
                logger.LogWarning("User {UserId} not found for demotion", userId);
                return false;
            }

            // Check if user is actually an admin
            if (!await userManager.IsInRoleAsync(userToDemote, "Admin"))
            {
                logger.LogInformation("User {UserId} is not an admin", userId);
                return true;
            }

            // Check if this would leave no admins
            var adminUsers = await userManager.GetUsersInRoleAsync("Admin");
            if (adminUsers.Count <= 1)
            {
                logger.LogWarning("Cannot demote user {UserId} - would leave no admins in the system", userId);
                return false;
            }

            // Remove user from Admin role
            var result = await userManager.RemoveFromRoleAsync(userToDemote, "Admin");
            if (result.Succeeded)
            {
                // Ensure user has User role
                if (!await userManager.IsInRoleAsync(userToDemote, "User"))
                {
                    if (!await roleManager.RoleExistsAsync("User"))
                    {
                        await roleManager.CreateAsync(new IdentityRole("User"));
                    }

                    await userManager.AddToRoleAsync(userToDemote, "User");
                }

                logger.LogInformation("User {UserId} ({Email}) demoted from Admin by {CurrentAdminId}",
                    userId, userToDemote.Email, currentAdminId);
                return true;
            }

            logger.LogError("Failed to demote user {UserId} from Admin: {Errors}",
                userId, string.Join(", ", result.Errors.Select(e => e.Description)));
            return false;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error demoting user {UserId} from Admin", userId);
            return false;
        }
    }

    public async Task<bool> IsUserAdminAsync(string userId)
    {
        var user = await userManager.FindByIdAsync(userId);
        if (user == null)
            return false;

        return await userManager.IsInRoleAsync(user, "Admin");
    }

    public async Task<List<UserResponseDto>> GetAdminUsersAsync()
    {
        var adminUsers = await userManager.GetUsersInRoleAsync("Admin");

        return adminUsers.Select(user => new UserResponseDto
        {
            Id = Guid.Parse(user.Id),
            Email = user.Email!,
            FullName = $"{user.FirstName} {user.LastName}",
            Role = "Admin"
        }).OrderBy(u => u.Email).ToList();
    }
}