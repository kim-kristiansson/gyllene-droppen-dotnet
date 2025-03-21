using GylleneDroppen.Api.Dtos;
using GylleneDroppen.Api.Dtos.Event;
using GylleneDroppen.Api.Models;

namespace GylleneDroppen.Api.Mappers.Interfaces;

public interface IEventMapper
{
    Event ToEvent(CreateEventRequest request);
    EventAdminResponse ToEventAdminResponse(Event @event);
    EventUserResponse ToEventUserResponse(Event @event);
    List<EventUserResponse> ToEventUserResponse(List<Event> events);
    List<EventAdminResponse> ToEventAdminResponse(List<Event> events);
}