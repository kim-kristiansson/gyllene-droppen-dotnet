namespace GylleneDroppen.Infrastructure.Queries.Tasting;

public class RegisterTastingQuery
{
    public required Guid Id { get; set; }
    public required DateTime Deadline { get; set; }
    public required int Capacity { get; set; }
    public required int ParticipantCount { get; set; }
}