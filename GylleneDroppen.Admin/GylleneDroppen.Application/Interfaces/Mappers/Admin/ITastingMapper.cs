namespace GylleneDroppen.Application.Interfaces.Mappers.Admin;

public interface ITastingMapper
{
    Event ToEvent(CreateEventRequest request);
    EventAdminResponse ToEventAdminResponse(Event @event);
    EventUserResponse ToEventUserResponse(Event @event);
    List<EventUserResponse> ToEventUserResponse(List<Event> events);
    List<EventAdminResponse> ToEventAdminResponse(List<Event> events);
}