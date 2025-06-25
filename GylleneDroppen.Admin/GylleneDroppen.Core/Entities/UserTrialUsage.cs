using System.ComponentModel.DataAnnotations;

namespace GylleneDroppen.Core.Entities;

public class UserTrialUsage
{
    public Guid Id { get; set; }

    [Required]
    public string UserId { get; set; } = string.Empty;
    public ApplicationUser User { get; set; } = null!;

    [Required]
    [MaxLength(256)]
    public string Email { get; set; } = string.Empty; // Store email for abuse prevention

    public bool HasUsedTrial { get; set; } = false;

    public DateTime? TrialUsedDate { get; set; }

    public Guid? TrialUsedForEventId { get; set; }
    public TastingEvent? TrialUsedForEvent { get; set; }

    [MaxLength(500)]
    public string? Notes { get; set; }

    public DateTime CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
}