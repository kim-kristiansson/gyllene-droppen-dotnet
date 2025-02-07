namespace GylleneDroppen.Admin.Api.Models;

public class Event
{
    public required Guid Id { get; init; }
    public required DateTime StartTime { get; set; }
    public required DateTime EndTime { get; set; }
    public required string Location { get; set; }
    public required int MaxParticipants { get; set; }
    public required decimal? Price { get; set; }
    
    public required List<EventParticipant> Participants { get; init; }
}
