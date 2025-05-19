using System.ComponentModel.DataAnnotations;

namespace GylleneDroppen.Application.Dtos.Auth;

public class RegisterRequestDto
{
    [Required(ErrorMessage = "E-post är obligatoriskt.")]
    [EmailAddress(ErrorMessage = "Ogiltig e-postadress.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Lösenord är obligatoriskt.")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Lösenordet måste vara minst 6 tecken.")]
    public string Password { get; set; } = string.Empty;

    [Required(ErrorMessage = "Bekräfta lösenordet.")]
    [Compare(nameof(Password), ErrorMessage = "Lösenorden matchar inte.")]
    public string ConfirmPassword { get; set; } = string.Empty;

    [Required(ErrorMessage = "Förnamn är obligatoriskt.")]
    [MaxLength(50, ErrorMessage = "Förnamnet får vara högst 50 tecken.")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Efternamn är obligatoriskt.")]
    [MaxLength(50, ErrorMessage = "Efternamnet får vara högst 50 tecken.")]
    public string LastName { get; set; } = string.Empty;
}