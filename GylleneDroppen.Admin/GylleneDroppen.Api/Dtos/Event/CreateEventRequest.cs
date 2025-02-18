using GylleneDroppen.Api.Models;

namespace GylleneDroppen.Api.Dtos;

public class CreateEventRequest
{
    public required string Title { get; init; }
    public required string Description { get; init; }
    public required string Location { get; init; }
    public required DateTime StartTime { get; init; }
    public required DateTime EndTime { get; init; }
    public required int Capacity { get; init; }
    public required int Price { get; init; }
    public required DateTime Deadline { get; init; }
    public required Guid OrganizerId { get; init; }
    public required Guid CreatedById { get; init; }
}

