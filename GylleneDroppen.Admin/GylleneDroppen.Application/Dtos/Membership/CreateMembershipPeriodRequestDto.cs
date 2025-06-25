using System.ComponentModel.DataAnnotations;

namespace GylleneDroppen.Application.Dtos.Membership;

public class CreateMembershipPeriodRequestDto
{
    [Required(ErrorMessage = "Namn är obligatoriskt.")]
    [MaxLength(100, ErrorMessage = "Namnet får vara högst 100 tecken.")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Varaktighet i månader är obligatorisk.")]
    [Range(1, 60, ErrorMessage = "Varaktighet måste vara mellan 1 och 60 månader.")]
    public int DurationInMonths { get; set; }

    [Required(ErrorMessage = "Pris är obligatoriskt.")]
    [Range(0, double.MaxValue, ErrorMessage = "Priset måste vara 0 eller högre.")]
    public decimal Price { get; set; }

    public bool IsActive { get; set; } = true;
}