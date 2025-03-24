using GylleneDroppen.Api.Dtos.WhiskyTasting;
using GylleneDroppen.Api.Interfaces.Mappers;
using GylleneDroppen.Api.Interfaces.Repositories;
using GylleneDroppen.Api.Interfaces.Services;
using GylleneDroppen.Core.Entities;
using GylleneDroppen.Core.Interfaces.Repositories;
using GylleneDroppen.Shared.Dtos.Generic;
using GylleneDroppen.Shared.Utils;

namespace GylleneDroppen.Api.Services;

public class WhiskyTastingService(
    IWhiskyTastingRepository whiskyTastingRepository,
    IWhiskyTastingMapper tastingMapper,
    IAttendeeRepository attendeeRepository)
    : IWhiskyTastingService
{
    public async Task<ServiceResponse<List<WhiskyTastingResponse>>> GetUpcomingEventsAsync()
    {
        var query = await whiskyTastingRepository.GetUpcomingWhiskyTastingAsync();

        var response = tastingMapper.ToWhiskyTastingResponse(query);

        return ServiceResponse<List<WhiskyTastingResponse>>.Success(response);
    }

    public async Task<ServiceResponse<MessageResponse>> RegisterForWhiskyTastingAsync(
        RegisterForWhiskyTastingRequest request,
        Guid userId)
    {
        var @event = await whiskyTastingRepository.GetRegisterWhiskyTastingDataAsync(request.WhiskyTastingId);

        if (@event == null)
            return ServiceResponse<MessageResponse>.Failure("WhiskyTasting not found.", 404);

        if (DateTime.UtcNow > @event.Deadline)
            return ServiceResponse<MessageResponse>.Failure("Registration deadline has passed.", 400);

        if (@event.AttendeeCount >= @event.Capacity)
            return ServiceResponse<MessageResponse>.Failure("This whisky tasting is fully booked.", 400);

        if (await attendeeRepository.IsUserRegisteredAsync(userId, request.WhiskyTastingId))
            return ServiceResponse<MessageResponse>.Failure("You are already registered for this event.", 400);

        var registration = new Attendee
        {
            UserId = userId,
            EventId = request.WhiskyTastingId,
            RegisteredAt = DateTime.UtcNow
        };

        await attendeeRepository.AddAsync(registration);

        return ServiceResponse<MessageResponse>.Success(new MessageResponse("Successfully registered for the event."));
    }
}