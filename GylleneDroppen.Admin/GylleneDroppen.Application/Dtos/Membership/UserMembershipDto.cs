namespace GylleneDroppen.Application.Dtos.Membership;

public class UserMembershipDto
{
    public Guid Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string UserEmail { get; set; } = string.Empty;
    public Guid MembershipPeriodId { get; set; }
    public string MembershipPeriodName { get; set; } = string.Empty;
    public int DurationInMonths { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public decimal AmountPaid { get; set; }
    public bool IsActive { get; set; }
    public bool IsExpired { get; set; }
    public bool IsCurrentlyValid { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedDate { get; set; }
    public string CreatedByUserName { get; set; } = string.Empty;
    public int DaysRemaining { get; set; }
}