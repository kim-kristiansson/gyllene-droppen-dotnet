using GylleneDroppen.Api.Dtos;
using GylleneDroppen.Api.Utilities;

namespace GylleneDroppen.Api.Services.Interfaces;

public interface IEventService
{
    Task<ServiceResponse<EventAdminResponse>> CreateEventAsync(CreateEventRequest createEventRequest);
    Task<ServiceResponse<List<EventUserResponse>>> GetUpcomingEvents();
}