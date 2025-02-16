using GylleneDroppen.Api.Dtos;
using GylleneDroppen.Api.Mappers.Interfaces;
using GylleneDroppen.Api.Models;

namespace GylleneDroppen.Api.Mappers;

public class EventMapper : IEventMapper
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
            CreatedAt = request.CreatedAt,
            Price = request.Price,
            Deadline = request.Deadline,
            Organizer = request.Organizer,
            CreatedBy = request.CreatedBy,
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
            Organizer = @event.Organizer,
            CreatedBy = @event.CreatedBy,
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
            Deadline = @event.Deadline,
            Organizer = @event.Organizer,
        };
    }

    public List<EventUserResponse> ToEventUserResponse(List<Event> events)
    {
        return events.Select(ToEventUserResponse).ToList();
    }
}