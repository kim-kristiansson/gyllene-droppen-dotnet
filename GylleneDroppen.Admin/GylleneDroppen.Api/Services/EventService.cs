using GylleneDroppen.Api.Dtos;
using GylleneDroppen.Api.Models;
using GylleneDroppen.Api.Repositories.Interfaces;
using GylleneDroppen.Api.Services.Interfaces;
using GylleneDroppen.Api.Utilities;

namespace GylleneDroppen.Api.Services;

public class EventService(IEventRepository eventRepository) : IEventService
{
    public async Task<ServiceResponse<EventResponse>> CreateEventAsync(CreateEventRequest createEventRequest)
    {
        var newEvent = new Event
        {
            Id = Guid.NewGuid(),
            Title = createEventRequest.Title,
            Description = createEventRequest.Description,
            Location = createEventRequest.Location,
            StartTime = createEventRequest.StartTime,
            EndTime = createEventRequest.EndTime
        };
        
        await eventRepository.AddAsync(newEvent);
        await eventRepository.SaveChangesAsync();

        var response = new EventResponse
        {
            Id = newEvent.Id,
            Title = newEvent.Title,
            Description = newEvent.Description,
            Location = newEvent.Location,
            StartTime = newEvent.StartTime,
            EndTime = newEvent.EndTime
        };

        return ServiceResponse<EventResponse>.Success(response);
    }

    public async Task<ServiceResponse<List<EventResponse>>> GetUpcomingEvents()
    {
        var upcomingEvents = await eventRepository.GetUpcomingEventsAsync();
        
        var response = upcomingEvents.Select(e => new EventResponse
        {
            Id = e.Id,
            Title = e.Title,
            Description = e.Description,
            Location = e.Location,
            StartTime = e.StartTime,
            EndTime = e.EndTime,
        }).ToList();
        
        return ServiceResponse<List<EventResponse>>.Success(response);
    }
}