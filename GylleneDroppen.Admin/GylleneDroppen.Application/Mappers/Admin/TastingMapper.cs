namespace GylleneDroppen.Application.Mappers.Admin;

public class EventMapper : ITastingMapper
{
    public Event ToEvent(CreateEventRequest request)
    {
        return new Event
        {
            Id = Guid.NewGuid(),
            Title = request.Title,
            Description = request.Description,
            Location = request.Location,
            StartTime = request.StartTime,
            EndTime = request.EndTime,
            Capacity = request.Capacity,
            CreatedAt = DateTime.UtcNow,
            Price = request.Price,
            Deadline = request.Deadline,
            OrganizerId = request.OrganizerId,
            CreatedById = request.CreatedById,
            Participants = []
        };
    }

    public EventAdminResponse ToEventAdminResponse(Event @event)
    {
        return new EventAdminResponse
        {
            Id = @event.Id,
            Title = @event.Title,
            Description = @event.Description,
            Location = @event.Location,
            StartTime = @event.StartTime,
            EndTime = @event.EndTime,
            Capacity = @event.Capacity,
            CreatedAt = @event.CreatedAt,
            Price = @event.Price,
            Deadline = @event.Deadline,
            OrganizerId = @event.OrganizerId,
            CreatedById = @event.CreatedById
        };
    }

    public EventUserResponse ToEventUserResponse(Event @event)
    {
        return new EventUserResponse
        {
            Id = @event.Id,
            Title = @event.Title,
            Description = @event.Description,
            StartTime = @event.StartTime,
            EndTime = @event.EndTime,
            Location = @event.Location,
            Capacity = @event.Capacity,
            Price = @event.Price,
            Deadline = @event.Deadline
        };
    }

    public List<EventUserResponse> ToEventUserResponse(List<Event> events)
    {
        return events.Select(ToEventUserResponse).ToList();
    }

    public List<EventAdminResponse> ToEventAdminResponse(List<Event> events)
    {
        return events.Select(ToEventAdminResponse).ToList();
    }
}