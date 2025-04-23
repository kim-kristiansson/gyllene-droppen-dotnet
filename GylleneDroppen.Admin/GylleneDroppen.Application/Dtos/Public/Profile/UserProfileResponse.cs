namespace GylleneDroppen.Application.Dtos.Public.Profile;

public class UserProfileResponse
{
    public required Guid Id { get; init; }
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public required string Email { get; init; }
    public required DateTime CreatedAt { get; init; }
    public required string Role { get; init; }
}