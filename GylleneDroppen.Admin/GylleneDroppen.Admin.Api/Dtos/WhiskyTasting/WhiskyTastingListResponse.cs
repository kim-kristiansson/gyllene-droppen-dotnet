namespace GylleneDroppen.Admin.Api.Dtos.WhiskyTasting;

public class WhiskyTastingListResponse
{
    public required Guid Id { get; init; }
    public required string Title { get; init; }
    public required DateTime Date { get; init; }
    public required string Location { get; init; }
    public required decimal Price { get; init; }
    public required int Capacity { get; init; }
    public required int RegisteredAttendees { get; init; }
    public required bool IsFull { get; init; }
    public required DateTime CreatedAt { get; init; }
    public required string CreatedBy { get; init; }
    public required bool IsPublished { get; init; }
}