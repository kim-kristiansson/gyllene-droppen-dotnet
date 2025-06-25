using System.ComponentModel.DataAnnotations;

namespace GylleneDroppen.Core.Entities;

public class UserMembership
{
    public Guid Id { get; set; }

    [Required]
    public string UserId { get; set; } = string.Empty;
    public ApplicationUser User { get; set; } = null!;

    [Required]
    public Guid MembershipPeriodId { get; set; }
    public MembershipPeriod MembershipPeriod { get; set; } = null!;

    [Required]
    public DateTime StartDate { get; set; }

    [Required]
    public DateTime EndDate { get; set; }

    [Required]
    [Range(0, double.MaxValue)]
    public decimal AmountPaid { get; set; }

    public bool IsActive { get; set; } = true;

    [MaxLength(500)]
    public string? Notes { get; set; }

    public DateTime CreatedDate { get; set; }

    [Required]
    public string CreatedByUserId { get; set; } = string.Empty;
    public ApplicationUser CreatedByUser { get; set; } = null!;

    // Computed property
    public bool IsExpired => DateTime.UtcNow > EndDate;
    public bool IsCurrentlyValid => IsActive && !IsExpired && DateTime.UtcNow >= StartDate;
}