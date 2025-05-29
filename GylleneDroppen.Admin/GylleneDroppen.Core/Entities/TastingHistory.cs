using System.ComponentModel.DataAnnotations;

namespace GylleneDroppen.Core.Entities;

public class TastingHistory
{
    public Guid Id { get; set; }

    [Required] public Guid WhiskyId { get; set; }
    public Whisky Whisky { get; set; } = null!;

    [Required] [MaxLength(200)] public string EventTitle { get; set; } = string.Empty;

    public DateTime TastingDate { get; set; }

    [MaxLength(1000)] public string? Notes { get; set; }

    [Required] public string OrganizedByUserId { get; set; } = string.Empty;
    public ApplicationUser OrganizedByUser { get; set; } = null!;

    public DateTime CreatedDate { get; set; }
}