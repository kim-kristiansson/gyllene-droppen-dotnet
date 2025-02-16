using GylleneDroppen.Api.Dtos;
using GylleneDroppen.Api.Extensions;
using GylleneDroppen.Api.Utilities;

namespace GylleneDroppen.Api.Services.Interfaces;

public interface IEventService
{
    Task<ServiceResponse<EventAdminResponse>> CreateEventAsync(CreateEventRequest request);
    Task<ServiceResponse<List<EventUserResponse>>> GetUpcomingEventsAsync();
    Task<ServiceResponse<List<EventAdminResponse>>> GetAllEventsAsync();
    Task<ServiceResponse<MessageResponse>> RegisterForEventAsync(RegisterForEventRequest request, Guid userId);
    Task<ServiceResponse<MessageResponse>> UpdateEventAsync(UpdateEventRequest request);
}