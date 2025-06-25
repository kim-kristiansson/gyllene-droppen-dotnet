using System.ComponentModel.DataAnnotations;

namespace GylleneDroppen.Application.Dtos.Membership;

public class CreateUserMembershipRequestDto
{
    [Required(ErrorMessage = "Användare är obligatorisk.")]
    public string UserId { get; set; } = string.Empty;

    [Required(ErrorMessage = "Medlemskapsperiod är obligatorisk.")]
    public Guid MembershipPeriodId { get; set; }

    [Required(ErrorMessage = "Startdatum är obligatoriskt.")]
    public DateTime StartDate { get; set; }

    [Required(ErrorMessage = "Betalt belopp är obligatoriskt.")]
    [Range(0, double.MaxValue, ErrorMessage = "Betalt belopp måste vara 0 eller högre.")]
    public decimal AmountPaid { get; set; }

    public bool IsActive { get; set; } = true;

    [MaxLength(500, ErrorMessage = "Anteckningar får vara högst 500 tecken.")]
    public string? Notes { get; set; }
}