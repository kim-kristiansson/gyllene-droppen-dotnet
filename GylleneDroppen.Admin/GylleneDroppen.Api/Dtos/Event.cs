namespace GylleneDroppen.Api.Dtos;

public class EventResponse
{
    public required Guid Id { get; init; }
    public required string Title { get; init; }
    public required string Description { get; init; }
    public required string Location { get; init; }
    public required DateTime StartTime { get; init; }
    public required DateTime EndTime { get; init; }
}