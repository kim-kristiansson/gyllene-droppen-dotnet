using System.ComponentModel.DataAnnotations;

namespace GylleneDroppen.Application.Dtos.Tasting;

public class CreateTastingHistoryRequestDto
{
    [Required] public Guid WhiskyId { get; set; }

    [Required(ErrorMessage = "Eventnamn är obligatoriskt.")]
    [MaxLength(200, ErrorMessage = "Titeln får vara högst 200 tecken.")]
    public string EventTitle { get; set; } = string.Empty;

    [Required(ErrorMessage = "Provningsdatum är obligatoriskt.")]
    public DateTime TastingDate { get; set; }

    [MaxLength(1000, ErrorMessage = "Anteckningarna får vara högst 1000 tecken.")]
    public string? Notes { get; set; }
}