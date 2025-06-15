using System.ComponentModel.DataAnnotations;
using GylleneDroppen.Core.Entities;

public class TastingEvent
{
    public Guid Id { get; set; }

    [Required] [MaxLength(200)] public string Title { get; set; } = string.Empty;

    [MaxLength(1000)] public string? Description { get; set; }

    [Required] public DateTime EventDate { get; set; }

    [MaxLength(200)] public string? Location { get; set; }

    [Range(1, 100)] public int? MaxParticipants { get; set; }

    public bool IsPublic { get; set; } = true;

    [Required] public string OrganizedByUserId { get; set; } = string.Empty;
    public ApplicationUser OrganizedByUser { get; set; } = null!;

    public DateTime CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }

    public ICollection<TastingEventWhisky> TastingEventWhiskies { get; set; } = new List<TastingEventWhisky>();
    public ICollection<TastingEventParticipant> Participants { get; set; } = new List<TastingEventParticipant>();
}