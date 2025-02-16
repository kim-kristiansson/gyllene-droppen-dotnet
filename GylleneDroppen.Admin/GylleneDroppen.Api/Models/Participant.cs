namespace GylleneDroppen.Api.Models;

public class Participant
{
    public required Guid UserId { get; init; }
    public required Guid EventId { get; init; }
    public required DateTime RegisteredAt { get; init; }
}