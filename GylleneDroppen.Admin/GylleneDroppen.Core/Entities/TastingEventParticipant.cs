using System.ComponentModel.DataAnnotations;

namespace GylleneDroppen.Core.Entities;

public class TastingEventParticipant
{
    public Guid Id { get; set; }

    [Required] public Guid TastingEventId { get; set; }
    public TastingEvent TastingEvent { get; set; } = null!;

    [Required] public string UserId { get; set; } = string.Empty;
    public ApplicationUser User { get; set; } = null!;

    public DateTime RegisteredDate { get; set; }

    public bool Attended { get; set; } = false;

    [MaxLength(1000)] public string? Notes { get; set; }
}