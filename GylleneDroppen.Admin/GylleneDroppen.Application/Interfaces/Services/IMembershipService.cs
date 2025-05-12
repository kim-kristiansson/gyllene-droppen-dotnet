using GylleneDroppen.Application.Common.Results;
using GylleneDroppen.Application.Dtos.Common;
using GylleneDroppen.Application.Dtos.Membership;

namespace GylleneDroppen.Application.Interfaces.Services;

public interface IMembershipService
{
    Task<Result<MessageResponse>> ActivateFreeTrialAsync(Guid userId);

    Task<Result<MessageResponse>> CreateMembershipAsync(Guid userId, CreateMembershipRequest request,
        Guid? adminId = null);

    Task<Result<MessageResponse>> CancelMembershipAsync(Guid userId);
    Task<Result<MembershipStatusResponse>> GetMembershipStatusAsync(Guid userId);

    Task<Result<MessageResponse>> UpdateMembershipAsync(Guid userId, UpdateMembershipRequest request,
        Guid? adminId = null);

    Task<Result<bool>> CanUserRegisterForTastingAsync(Guid userId);
}