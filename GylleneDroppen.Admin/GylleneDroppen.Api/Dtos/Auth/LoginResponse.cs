namespace GylleneDroppen.Api.Dtos.Auth;


public class LoginResponse
{
    public required Guid Id { get; init; }
    public required string Email { get; init; }
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public required string AccessToken { get; init; }
    public required string RefreshToken { get; init; }
}