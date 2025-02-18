using System.ComponentModel.DataAnnotations;

namespace GylleneDroppen.Api.Dtos;

public class LoginRequest
{
    [EmailAddress]
    public required string Email { get; init; }
    
    [DataType(DataType.Password)]
    public required string Password { get; init; }
}

public class LoginResponse
{
    public required Guid Id { get; init; }
    public required string Email { get; init; }
    public required string AccessToken { get; init; }
    public required string RefreshToken { get; init; }
}