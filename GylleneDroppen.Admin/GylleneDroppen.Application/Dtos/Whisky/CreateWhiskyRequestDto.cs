using System.ComponentModel.DataAnnotations;

namespace GylleneDroppen.Application.Dtos.Whisky;

public class CreateWhiskyRequestDto
{
    [Required(ErrorMessage = "Namn är obligatoriskt.")]
    [MaxLength(200, ErrorMessage = "Namnet får vara högst 200 tecken.")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Destilleri är obligatoriskt.")]
    [MaxLength(200, ErrorMessage = "Destilleriet får vara högst 200 tecken.")]
    public string Distillery { get; set; } = string.Empty;

    [Range(0, 100, ErrorMessage = "Ålder måste vara mellan 0 och 100 år.")]
    public int? Age { get; set; }

    [Required(ErrorMessage = "Alkoholhalt är obligatorisk.")]
    [Range(0, 100, ErrorMessage = "Alkoholhalt måste vara mellan 0 och 100%.")]
    public decimal Abv { get; set; }

    public Guid? RegionId { get; set; }

    public Guid? WhiskyTypeId { get; set; }

    // Country is derived through Region - no direct selection needed

    [MaxLength(500, ErrorMessage = "Färgbeskrivningen får vara högst 500 tecken.")]
    public string? Color { get; set; }

    [MaxLength(1000, ErrorMessage = "Doftbeskrivningen får vara högst 1000 tecken.")]
    public string? Nose { get; set; }

    [MaxLength(1000, ErrorMessage = "Smakbeskrivningen får vara högst 1000 tecken.")]
    public string? Palate { get; set; }

    [MaxLength(1000, ErrorMessage = "Eftersmaksbeskrivningen får vara högst 1000 tecken.")]
    public string? Finish { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "Priset måste vara positivt.")]
    public decimal? Price { get; set; }

    [Range(1, 10000, ErrorMessage = "Flaskstorlek måste vara mellan 1 och 10000 ml.")]
    public int? BottleSize { get; set; }
}