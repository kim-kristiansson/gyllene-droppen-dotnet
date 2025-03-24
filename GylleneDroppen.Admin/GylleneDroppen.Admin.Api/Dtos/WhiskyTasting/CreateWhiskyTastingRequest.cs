using GylleneDroppen.Core.Entities;

namespace GylleneDroppen.Admin.Api.Dtos.WhiskyTasting;

public class CreateWhiskyTastingRequest
{
    public required Guid Id { get; init; }
    public required string Title { get; init; }
    public required string Description { get; init; }
    public required string Location { get; init; }
    public required DateTime StartTime { get; init; }
    public required DateTime EndTime { get; init; }
    public required int Capacity { get; init; }
    public required DateTime CreatedAt { get; init; }
    public required decimal Price { get; init; }
    public required DateTime Deadline { get; init; }
    public Guid OrganizerId { get; init; }
    public Guid CreatedById { get; init; }
    public List<Attendee> Attendees { get; init; } = [];
}