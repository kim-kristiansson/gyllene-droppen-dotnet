using GylleneDroppen.Api.Dtos;
using GylleneDroppen.Api.Dtos.Event;
using GylleneDroppen.Api.Extensions;
using GylleneDroppen.Api.Mappers.Interfaces;
using GylleneDroppen.Api.Models;
using GylleneDroppen.Api.Repositories.Interfaces;
using GylleneDroppen.Api.Services.Interfaces;
using GylleneDroppen.Api.Utilities;

namespace GylleneDroppen.Api.Services;

public class EventService(
    IEventRepository eventRepository, 
    IEventMapper eventMapper, 
    IParticipantRepository participantRepository) 
    : IEventService
{
    public async Task<ServiceResponse<MessageResponse>> CreateEventAsync(CreateEventRequest request)
    {
        if (request.EndTime <= request.StartTime)
            return ServiceResponse<MessageResponse>.Failure("End time must be after start time.", 400);
        
        if (request.Deadline >= request.StartTime)
            return ServiceResponse<MessageResponse>.Failure("Deadline must be before event start time.", 400);
        
        var newEvent = eventMapper.ToEvent(request);
        
        await eventRepository.AddAsync(newEvent);
        await eventRepository.SaveChangesAsync();

        return ServiceResponse<MessageResponse>.Success(new MessageResponse("Event created successfully."));
    }

    public async Task<ServiceResponse<List<EventUserResponse>>> GetUpcomingEventsAsync()
    {
        var response = await eventRepository.GetUpcomingEventsAsync();
        
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
        var @event = await eventRepository.GetRegisterEventDataAsync(request.EventId);
        
        if(@event == null)
            return ServiceResponse<MessageResponse>.Failure("Event not found.", 404);
        
        if(DateTime.UtcNow > @event.Deadline)
            return ServiceResponse<MessageResponse>.Failure("Registration deadline has passed.", 400);
        
        if(@event.ParticipantCount >= @event.Capacity)
            return ServiceResponse<MessageResponse>.Failure("This event is fully booked.", 400);

        if (await participantRepository.IsUserRegisteredAsync(userId, request.EventId))
            return ServiceResponse<MessageResponse>.Failure("You are already registered for this event.", 400);
        
        var registration = new Participant
        {
            UserId = userId,
            EventId = request.EventId,
            RegisteredAt = DateTime.UtcNow
        };
        
        await participantRepository.AddAsync(registration);
        
        return ServiceResponse<MessageResponse>.Success(new MessageResponse("Successfully registered for the event."));
    }

    public async Task<ServiceResponse<MessageResponse>> UpdateEventAsync(UpdateEventRequest eventRequest)
    {
        var existingEvent = await eventRepository.GetByIdAsync(eventRequest.Id);
        if(existingEvent is null)
            return ServiceResponse<MessageResponse>.Failure("Event not found.", 404);
        
        if (eventRequest.EndTime <= eventRequest.StartTime)
            return ServiceResponse<MessageResponse>.Failure("End time must be after start time.", 400);
        
        if (eventRequest.Deadline >= eventRequest.StartTime)
            return ServiceResponse<MessageResponse>.Failure("Deadline must be before event start time.", 400);
        
        if (existingEvent.Title != eventRequest.Title)
            existingEvent.Title = eventRequest.Title;
    
        if (existingEvent.Description != eventRequest.Description)
            existingEvent.Description = eventRequest.Description;
    
        if (existingEvent.Location != eventRequest.Location)
            existingEvent.Location = eventRequest.Location;
    
        if (existingEvent.StartTime != eventRequest.StartTime)
            existingEvent.StartTime = eventRequest.StartTime;
    
        if (existingEvent.EndTime != eventRequest.EndTime)
            existingEvent.EndTime = eventRequest.EndTime;
    
        if (existingEvent.Capacity != eventRequest.Capacity)
            existingEvent.Capacity = eventRequest.Capacity;
    
        if (existingEvent.Price != eventRequest.Price)
            existingEvent.Price = eventRequest.Price;
    
        if (existingEvent.Deadline != eventRequest.Deadline)
            existingEvent.Deadline = eventRequest.Deadline;
        
        if (existingEvent.OrganizerId != eventRequest.OrganizerId)
            existingEvent.OrganizerId = eventRequest.OrganizerId;
        
        eventRepository.Update(existingEvent);
        await eventRepository.SaveChangesAsync();
        
        return ServiceResponse<MessageResponse>.Success(new MessageResponse("Successfully updated event."));
    }
}