using System.ComponentModel.DataAnnotations;

namespace GylleneDroppen.Shared.Dtos.Auth;

public class LoginRequest
{
    [EmailAddress] public required string Email { get; init; }

    [DataType(DataType.Password)] public required string Password { get; init; }
}