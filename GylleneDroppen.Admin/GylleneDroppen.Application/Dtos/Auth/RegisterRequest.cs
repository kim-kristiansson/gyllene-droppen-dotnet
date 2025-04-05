using System.ComponentModel.DataAnnotations;

namespace GylleneDroppen.Application.Dtos.Auth;

public class RegisterRequest
{
    [EmailAddress] public required string Email { get; init; }

    [MaxLength(100)] public required string FirstName { get; init; }

    [MaxLength(100)] public required string LastName { get; init; }

    [DataType(DataType.Password)]
    [MinLength(6)]
    public required string Password { get; init; }

    [DataType(DataType.Password)]
    [Compare("Password")]
    public required string ConfirmPassword { get; init; }
}