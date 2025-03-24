namespace GylleneDroppen.Core.Entities;

public class Attendee
{
    public required Guid UserId { get; init; }
    public User? User { get; init; }
    public required Guid EventId { get; init; }
    public Event? Event { get; init; }
    public required DateTime RegisteredAt { get; init; }
}