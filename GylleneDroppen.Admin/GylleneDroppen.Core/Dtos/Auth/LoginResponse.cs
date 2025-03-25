namespace GylleneDroppen.Core.Dtos.Auth;

public class LoginResponse
{
    public required Guid Id { get; set; }
    public required string Email { get; set; }
    public required string Name { get; set; }
    public required string Role { get; set; }
}