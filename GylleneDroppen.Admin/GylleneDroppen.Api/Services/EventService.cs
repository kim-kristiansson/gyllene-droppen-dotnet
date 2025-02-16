using GylleneDroppen.Api.Dtos;
using GylleneDroppen.Api.Mappers.Interfaces;
using GylleneDroppen.Api.Repositories.Interfaces;
using GylleneDroppen.Api.Services.Interfaces;
using GylleneDroppen.Api.Utilities;

namespace GylleneDroppen.Api.Services;

public class EventService(IEventRepository eventRepository, IEventMapper eventMapper) : IEventService
{
    public async Task<ServiceResponse<EventAdminResponse>> CreateEventAsync(CreateEventRequest createEventRequest)
    {
        var newEvent = eventMapper.ToEvent(createEventRequest);
        
        await eventRepository.AddAsync(newEvent);
        await eventRepository.SaveChangesAsync();

        var response = eventMapper.ToEventAdminResponse(newEvent);

        return ServiceResponse<EventAdminResponse>.Success(response);
    }

    public async Task<ServiceResponse<List<EventUserResponse>>> GetUpcomingEvents()
    {
        var upcomingEvents = await eventRepository.GetUpcomingEventsAsync();

        var response = eventMapper.ToEventUserResponse(upcomingEvents);
        
        return ServiceResponse<List<EventUserResponse>>.Success(response);
    }
}