namespace GylleneDroppen.Api.Queries.WhiskyTasting;

public class RegisterForWhiskyTastingQuery
{
    public required Guid Id { get; init; }
    public required DateTime Deadline { get; init; }
    public required int Capacity { get; init; }
    public required int AttendeeCount { get; init; }
}