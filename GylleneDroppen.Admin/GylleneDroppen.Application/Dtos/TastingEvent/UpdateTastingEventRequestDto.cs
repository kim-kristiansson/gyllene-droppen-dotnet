using System.ComponentModel.DataAnnotations;

namespace GylleneDroppen.Application.Dtos.TastingEvent;

public class UpdateTastingEventRequestDto
{
    [Required] public Guid Id { get; set; }

    [Required(ErrorMessage = "Titel är obligatorisk.")]
    [MaxLength(200, ErrorMessage = "Titeln får vara högst 200 tecken.")]
    public string Title { get; set; } = string.Empty;

    [MaxLength(1000, ErrorMessage = "Beskrivningen får vara högst 1000 tecken.")]
    public string? Description { get; set; }

    [Required(ErrorMessage = "Eventdatum är obligatoriskt.")]
    public DateTime EventDate { get; set; }

    [MaxLength(200, ErrorMessage = "Platsen får vara högst 200 tecken.")]
    public string? Location { get; set; }

    [Range(1, 100, ErrorMessage = "Max antal deltagare måste vara mellan 1 och 100.")]
    public int? MaxParticipants { get; set; }

    public bool IsPublic { get; set; }
}