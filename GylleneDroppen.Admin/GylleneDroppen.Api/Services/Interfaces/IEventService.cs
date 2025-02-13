using GylleneDroppen.Api.Dtos;
using GylleneDroppen.Api.Utilities;

namespace GylleneDroppen.Api.Services.Interfaces;

public interface IEventService
{
    Task<ServiceResponse<EventResponse>> CreateEventAsync(CreateEventRequest createEventRequest);
    Task<ServiceResponse<List<EventResponse>>> GetUpcomingEvents();
}