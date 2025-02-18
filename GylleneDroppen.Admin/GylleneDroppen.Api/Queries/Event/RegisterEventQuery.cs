namespace GylleneDroppen.Api.Queries.Event;

public class RegisterEventQuery
{
    public required Guid Id { get; set; }
    public required DateTime Deadline { get; set; }
    public required int Capacity { get; set; }
    public required int ParticipantCount { get; set; }
}