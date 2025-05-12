using GylleneDroppen.Domain.Enums;

namespace GylleneDroppen.Application.Dtos.Membership;

public class UpdateMembershipRequest
{
    public MembershipStatus? Status { get; init; }
    public DateTime? EndDate { get; init; }
    public string? Notes { get; init; }
}