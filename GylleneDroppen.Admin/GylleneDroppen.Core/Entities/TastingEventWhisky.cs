using System.ComponentModel.DataAnnotations;

namespace GylleneDroppen.Core.Entities;

public class TastingEventWhisky
{
    public Guid Id { get; set; }

    [Required] public Guid TastingEventId { get; set; }
    public TastingEvent TastingEvent { get; set; } = null!;

    [Required] public Guid WhiskyId { get; set; }
    public Whisky Whisky { get; set; } = null!;

    public int Order { get; set; } // Order in which whiskies will be tasted

    [MaxLength(1000)] public string? Notes { get; set; }

    public DateTime AddedDate { get; set; }

    [Required] public string AddedByUserId { get; set; } = string.Empty;
    public ApplicationUser AddedByUser { get; set; } = null!;
}