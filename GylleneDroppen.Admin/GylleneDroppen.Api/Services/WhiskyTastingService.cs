using GylleneDroppen.Api.Dtos.WhiskyTasting;
using GylleneDroppen.Api.Interfaces.Mappers;
using GylleneDroppen.Api.Interfaces.Repositories;
using GylleneDroppen.Api.Interfaces.Services;
using GylleneDroppen.Core.Common;
using GylleneDroppen.Core.Dtos.Generic;
using GylleneDroppen.Core.Entities;
using GylleneDroppen.Core.Interfaces.Repositories;

namespace GylleneDroppen.Api.Services;

public class WhiskyTastingService(
    IWhiskyTastingRepository whiskyTastingRepository,
    IWhiskyTastingMapper tastingMapper,
    IAttendeeRepository attendeeRepository)
    : IWhiskyTastingService
{
    public async Task<Result<List<WhiskyTastingResponse>>> GetUpcomingEventsAsync()
    {
        var query = await whiskyTastingRepository.GetUpcomingWhiskyTastingAsync();

        var response = tastingMapper.ToWhiskyTastingResponse(query);

        return Result<List<WhiskyTastingResponse>>.Success(response);
    }

    public async Task<Result<MessageResponse>> RegisterForWhiskyTastingAsync(
        RegisterForWhiskyTastingRequest request,
        Guid userId)
    {
        var @event = await whiskyTastingRepository.GetRegisterWhiskyTastingDataAsync(request.WhiskyTastingId);

        if (@event == null)
            return Result<MessageResponse>.Failure("WhiskyTasting not found.", 404);

        if (DateTime.UtcNow > @event.Deadline)
            return Result<MessageResponse>.Failure("Registration deadline has passed.", 400);

        if (@event.AttendeeCount >= @event.Capacity)
            return Result<MessageResponse>.Failure("This whisky tasting is fully booked.", 400);

        if (await attendeeRepository.IsUserRegisteredAsync(userId, request.WhiskyTastingId))
            return Result<MessageResponse>.Failure("You are already registered for this event.", 400);

        var registration = new Attendee
        {
            UserId = userId,
            EventId = request.WhiskyTastingId,
            RegisteredAt = DateTime.UtcNow
        };

        await attendeeRepository.AddAsync(registration);

        return Result<MessageResponse>.Success(new MessageResponse("Successfully registered for the event."));
    }
}