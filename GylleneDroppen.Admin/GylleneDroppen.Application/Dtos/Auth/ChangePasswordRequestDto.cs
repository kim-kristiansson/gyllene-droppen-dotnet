using System.ComponentModel.DataAnnotations;

namespace GylleneDroppen.Application.Dtos.Auth;

public class ChangePasswordRequestDto
{
    [Required(ErrorMessage = "Nuvarande lösenord är obligatoriskt.")]
    public string CurrentPassword { get; set; } = string.Empty;

    [Required(ErrorMessage = "Nytt lösenord är obligatoriskt.")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Lösenordet måste vara minst 6 tecken.")]
    public string NewPassword { get; set; } = string.Empty;

    [Required(ErrorMessage = "Bekräfta det nya lösenordet.")]
    [Compare(nameof(NewPassword), ErrorMessage = "Lösenorden matchar inte.")]
    public string ConfirmNewPassword { get; set; } = string.Empty;
}