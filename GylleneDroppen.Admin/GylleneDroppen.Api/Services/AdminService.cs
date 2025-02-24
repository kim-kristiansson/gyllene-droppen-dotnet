using GylleneDroppen.Api.Dtos;
using GylleneDroppen.Api.Dtos.Admin;
using GylleneDroppen.Api.Enums;
using GylleneDroppen.Api.Repositories.Interfaces;
using GylleneDroppen.Api.Services.Interfaces;
using GylleneDroppen.Api.Utilities;

namespace GylleneDroppen.Api.Services;

public class AdminService(IUserRepository userRepository) : IAdminService
{
    public async Task<ServiceResponse<MessageResponse>> PromoteUserToAdminAsync(Guid userId)
    {
        var user = await userRepository.GetByIdAsync(userId);
        
        if(user is null)
            return ServiceResponse<MessageResponse>.Failure("User not found.", 404);
        
        if(user.Role is RoleType.User)
            return ServiceResponse<MessageResponse>.Failure("User is already an admin.", 400);
        
        user.Role = RoleType.Admin; 
        userRepository.Update(user);
        await userRepository.SaveChangesAsync();
        
        return ServiceResponse<MessageResponse>.Success(new MessageResponse("User promoted to Admin."));
    }

    public async Task<ServiceResponse<MessageResponse>> DemoteUserToAdminAsync(DemoteAdminRequest request)
    {
        if(request.UserId == request.AdminId)
            return ServiceResponse<MessageResponse>.Success(new MessageResponse("You cannot demote your users."));
        
        var user = await userRepository.GetByIdAsync(request.UserId);
        if(user is null)
            return ServiceResponse<MessageResponse>.Failure("User not found.", 404);
        
        if(user.Role is not RoleType.Admin)
            return ServiceResponse<MessageResponse>.Failure("User is not an admin.", 400);
        
        user.Role = RoleType.User;
        userRepository.Update(user);
        await userRepository.SaveChangesAsync();
        
        return ServiceResponse<MessageResponse>.Success(new MessageResponse("User demoted to User."));
    }
}