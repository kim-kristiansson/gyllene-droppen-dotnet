namespace GylleneDroppen.Application.Dtos.Auth;

public class CurrentUserResponse
{
    public required Guid Id { get; init; }
    public required string Email { get; init; }
    public required string Role { get; init; }
}