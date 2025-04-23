namespace GylleneDroppen.Application.Dtos.Public.Profile;

public class UserTastingResponse
{
    public required Guid TastingId { get; init; }
    public required string Title { get; init; }
    public required string Description { get; init; }
    public required DateTime StartTime { get; init; }
    public required DateTime EndTime { get; init; }
    public required string Location { get; init; }
    public required DateTime RegisteredAt { get; init; }
    public required string Status { get; init; }
}