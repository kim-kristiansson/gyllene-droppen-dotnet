using GylleneDroppen.Domain.Enums;

namespace GylleneDroppen.Domain.Entities;

public class Membership
{
    public required Guid Id { get; init; }
    public required Guid UserId { get; init; }
    public User? User { get; init; }
    public required MembershipType Type { get; set; }
    public required MembershipStatus Status { get; set; }
    public required DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public string? PaymentReference { get; set; }
    public string? Notes { get; set; }

    public Guid? LastUpdatedById { get; set; }
}