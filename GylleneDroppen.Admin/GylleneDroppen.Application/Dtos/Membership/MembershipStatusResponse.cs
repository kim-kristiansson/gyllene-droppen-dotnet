using GylleneDroppen.Domain.Enums;

namespace GylleneDroppen.Application.Dtos.Membership;

public class MembershipStatusResponse
{
    public required bool HasActiveMembership { get; init; }
    public MembershipType? CurrentMembershipType { get; init; }
    public MembershipStatus? CurrentMembershipStatus { get; init; }
    public DateTime? MembershipStartDate { get; init; }
    public DateTime? MembershipEndDate { get; init; }
    public bool CanRegisterForTastings { get; init; }
    public bool HasValidFreeTrial { get; init; }
}