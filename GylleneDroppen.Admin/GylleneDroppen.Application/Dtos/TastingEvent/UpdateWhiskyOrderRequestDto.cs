using System.ComponentModel.DataAnnotations;

namespace GylleneDroppen.Application.Dtos.TastingEvent;

public class UpdateWhiskyOrderRequestDto
{
    [Required] public Guid TastingEventId { get; set; }

    [Required] public List<WhiskyOrderDto> WhiskyOrders { get; set; } = new();
}