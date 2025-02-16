namespace GylleneDroppen.Api.Models;

public class Event
{
    public required Guid Id { get; init; }
    public required DateTime CreatedAt { get; init; }
    public required string Title { get; set; }
    public required string Description { get; set; }
    public required DateTime StartTime { get; set; }
    public required DateTime EndTime { get; set; }
    public required string Location { get; set; }
    public required int Capacity { get; set; }
    public required decimal Price { get; set; }
    public required DateTime Deadline { get; set; }
    public required User Organizer { get; set; }
    public required User CreatedBy { get; init; }
    public required List<Participant> Participants { get; init; }
}