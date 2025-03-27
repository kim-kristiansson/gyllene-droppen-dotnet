namespace GylleneDroppen.Core.Domain.Entities;

public class Attendee
{
    public required Guid UserId { get; init; }
    public User? User { get; init; }
    public required Guid EventId { get; init; }
    public Tasting? Tasting { get; init; }
    public required DateTime RegisteredAt { get; init; }
}