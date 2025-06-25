namespace GylleneDroppen.Application.Dtos.Membership;

public class MembershipPeriodDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int DurationInMonths { get; set; }
    public decimal Price { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedDate { get; set; }
    public string CreatedByUserName { get; set; } = string.Empty;
}