using System.ComponentModel.DataAnnotations;

namespace GylleneDroppen.Application.Dtos.TastingEvent;

public class AddWhiskyToEventRequestDto
{
    [Required] public Guid TastingEventId { get; set; }

    [Required] public Guid WhiskyId { get; set; }

    public int Order { get; set; }

    [MaxLength(1000)] public string? Notes { get; set; }
}