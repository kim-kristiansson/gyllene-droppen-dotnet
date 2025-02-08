using System.ComponentModel.DataAnnotations;

namespace GylleneDroppen.Api.Dtos;

public class RegisterRequest
{
    [EmailAddress]
    public required string Email { get; init; }
    
    [DataType(DataType.Password)]
    [MinLength(6)]
    public required string Password { get; init; }
    
    [DataType(DataType.Password)]
    [Compare("Password")]
    public required string ConfirmPassword { get; init; }
}