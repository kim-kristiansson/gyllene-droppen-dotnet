using System.ComponentModel.DataAnnotations;

namespace GylleneDroppen.Application.Dtos.Shared.Auth;

public class RegisterRequest
{
    [EmailAddress] public required string Email { get; set; }

    [MaxLength(100)] public required string FirstName { get; set; }

    [MaxLength(100)] public required string LastName { get; set; }

    [DataType(DataType.Password)]
    [MinLength(6)]
    public required string Password { get; set; }

    [DataType(DataType.Password)]
    [Compare("Password")]
    public required string ConfirmPassword { get; set; }
}