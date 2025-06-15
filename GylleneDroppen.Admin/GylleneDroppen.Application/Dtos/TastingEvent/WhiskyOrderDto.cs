using System.ComponentModel.DataAnnotations;

namespace GylleneDroppen.Application.Dtos.TastingEvent;

public class WhiskyOrderDto
{
    [Required] public Guid WhiskyId { get; set; }
    public int Order { get; set; }
}