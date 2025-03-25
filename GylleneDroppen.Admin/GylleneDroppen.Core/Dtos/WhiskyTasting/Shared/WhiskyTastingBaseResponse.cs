using GylleneDroppen.Core.Dtos.Generic;

namespace GylleneDroppen.Core.Dtos.WhiskyTasting.Shared;

public class WhiskyTastingBaseResponse
{
    public required Guid Id { get; init; }
    public required string Title { get; init; }
    public required string Description { get; init; }
    public required DateTime StartTime { get; init; }
    public required DateTime EndTime { get; init; }
    public required string Location { get; init; }
    public required int Capacity { get; init; }
    public required decimal Price { get; init; }
    public required DateTime Deadline { get; init; }
    public required OrganizerResponse Organizer { get; init; }
}