using GylleneDroppen.Api.Dtos;
using GylleneDroppen.Api.Extensions;
using GylleneDroppen.Api.Mappers.Interfaces;
using GylleneDroppen.Api.Models;
using GylleneDroppen.Api.Repositories.Interfaces;
using GylleneDroppen.Api.Services.Interfaces;
using GylleneDroppen.Api.Utilities;

namespace GylleneDroppen.Api.Services;

public class EventService(IEventRepository eventRepository, IEventMapper eventMapper, IParticipantRepository participantRepository) : IEventService
{
    public async Task<ServiceResponse<EventAdminResponse>> CreateEventAsync(CreateEventRequest createEventRequest)
    {
        var newEvent = eventMapper.ToEvent(createEventRequest);
        
        await eventRepository.AddAsync(newEvent);
        await eventRepository.SaveChangesAsync();

        var response = eventMapper.ToEventAdminResponse(newEvent);

        return ServiceResponse<EventAdminResponse>.Success(response);
    }

    public async Task<ServiceResponse<List<EventUserResponse>>> GetUpcomingEventsAsync()
    {
        var upcomingEvents = await eventRepository.GetUpcomingEventsAsync();

        var response = eventMapper.ToEventUserResponse(upcomingEvents);
        
        return ServiceResponse<List<EventUserResponse>>.Success(response);
    }

    public async Task<ServiceResponse<List<EventAdminResponse>>> GetAllEventsAsync()
    {
        var events = await eventRepository.GetAllAsync();
        
        var response = eventMapper.ToEventAdminResponse(events);
        
        return ServiceResponse<List<EventAdminResponse>>.Success(response);
    }

    public async Task<ServiceResponse<MessageResponse>> RegisterForEventAsync(RegisterForEventRequest request, Guid userId)
    {
        var @event = await eventRepository.GetByIdAsync(request.EventId);
        
        if(@event == null)
            return ServiceResponse<MessageResponse>.Failure("Event not found.", 404);
        
        if(DateTime.UtcNow > @event.Deadline)
            return ServiceResponse<MessageResponse>.Failure("Registration deadline has passed.", 400);
        
        if(@event.Participants.Count >= @event.Capacity)
            return ServiceResponse<MessageResponse>.Failure("This event is fully booked.", 400);

        if (await participantRepository.IsUserRegisteredAsync(userId, request.EventId))
            return ServiceResponse<MessageResponse>.Failure("You are already registered for this event.", 400);
        
        var registration = new Participant
        {
            UserId = userId,
            EventId = request.EventId,
            RegisteredAt = DateTime.UtcNow,
        };
        
        await participantRepository.AddAsync(registration);
        
        return ServiceResponse<MessageResponse>.Success(new MessageResponse("Successfully registered for the event."));
    }

    public async Task<ServiceResponse<MessageResponse>> UpdateEventAsync(UpdateEventRequest request)
    {
        var existingEvent = await eventRepository.GetByIdAsync(request.Id);
        if(existingEvent is null)
            return ServiceResponse<MessageResponse>.Failure("Event not found.", 404);
        
        existingEvent.Title = request.Title;
        existingEvent.Description = request.Description;
        existingEvent.Location = request.Location;
        existingEvent.StartTime = request.StartTime;
        existingEvent.EndTime = request.EndTime;
        existingEvent.Capacity = request.Capacity;
        existingEvent.Price = request.Price;
        existingEvent.Deadline = request.Deadline;
        existingEvent.Organizer = request.Organizer;
        
        eventRepository.Update(existingEvent);
        await eventRepository.SaveChangesAsync();
        
        return ServiceResponse<MessageResponse>.Success(new MessageResponse("Successfully updated event."));
    }
}