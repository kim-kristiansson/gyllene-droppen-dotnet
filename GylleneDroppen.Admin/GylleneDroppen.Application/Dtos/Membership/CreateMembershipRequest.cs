using GylleneDroppen.Domain.Enums;

namespace GylleneDroppen.Application.Dtos.Membership;

public class CreateMembershipRequest
{
    public required MembershipType Type { get; init; }
    public required int DurationMonths { get; init; } = 12;
    public string? PaymentReference { get; init; }
    public string? Notes { get; init; }
}