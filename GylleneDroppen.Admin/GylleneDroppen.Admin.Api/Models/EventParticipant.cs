namespace GylleneDroppen.Admin.Api.Models;

public class EventParticipant
{
    public required Guid UserId { get; init; }
    public required User User { get; init; }
    
    public required Guid EventId { get; init; }
    public required Event Event { get; init; }
}