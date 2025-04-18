using GylleneDroppen.Application.Common.Results;
using GylleneDroppen.Application.Dtos.Admin;
using GylleneDroppen.Application.Dtos.Admin.Admin;
using GylleneDroppen.Application.Dtos.Common;
using GylleneDroppen.Application.Interfaces.Repositories.Admin;
using GylleneDroppen.Application.Interfaces.Services.Admin;
using GylleneDroppen.Domain.Enums;

namespace GylleneDroppen.Application.Services.Admin;

public class AdminService(IUserRepository userRepository) : IAdminService
{
    public async Task<Result<MessageResponse>> PromoteUserToAdminAsync(Guid userId)
    {
        var user = await userRepository.GetByIdAsync(userId);

        if (user is null)
            return Result<MessageResponse>.Failure("User not found.", 404);

        if (user.Role is RoleType.User)
            return Result<MessageResponse>.Failure("User is already an admin.", 400);

        user.Role = RoleType.Admin;
        userRepository.Update(user);
        await userRepository.SaveChangesAsync();

        return Result<MessageResponse>.Success(new MessageResponse("User promoted to Admin."));
    }

    public async Task<Result<MessageResponse>> DemoteUserToAdminAsync(DemoteAdminRequest request)
    {
        if (request.UserId == request.AdminId)
            return Result<MessageResponse>.Success(new MessageResponse("You cannot demote your users."));

        var user = await userRepository.GetByIdAsync(request.UserId);
        if (user is null)
            return Result<MessageResponse>.Failure("User not found.", 404);

        if (user.Role is not RoleType.Admin)
            return Result<MessageResponse>.Failure("User is not an admin.", 400);

        user.Role = RoleType.User;
        userRepository.Update(user);
        await userRepository.SaveChangesAsync();

        return Result<MessageResponse>.Success(new MessageResponse("User demoted to User."));
    }

    public async Task<Result<List<AdminResponse>>> GetAllAdminsAsync()
    {
        var admins = await userRepository.GetUsersByRoleAsync(RoleType.Admin);

        var response = admins.Select(user => new AdminResponse
        {
            Id = user.Id,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName
        }).ToList();

        return Result<List<AdminResponse>>.Success(response);
    }
}