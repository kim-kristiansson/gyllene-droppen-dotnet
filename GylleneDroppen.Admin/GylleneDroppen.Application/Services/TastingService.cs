using GylleneDroppen.Application.Common.Results;
using GylleneDroppen.Application.Dtos.Common;
using GylleneDroppen.Application.Dtos.Public.Tasting;
using GylleneDroppen.Application.Dtos.Tasting;
using GylleneDroppen.Application.Interfaces.Public.Mappers;
using GylleneDroppen.Application.Interfaces.Public.Services;
using GylleneDroppen.Application.Interfaces.Repositories;
using GylleneDroppen.Application.Interfaces.Shared.Repositories;
using GylleneDroppen.Domain.Entities;

namespace GylleneDroppen.Application.Services.Public;

public class TastingService(
    ITastingRepository tastingRepository,
    IAttendeeRepository attendeeRepository,
    ITastingMapper tastingMapper)
    : ITastingService
{
    public async Task<Result<MessageResponse>> RegisterForTastingAsync(Guid userId, RegisterForTastingRequest request)
    {
        var registerData = await tastingRepository.GetRegisterTastingDataAsync(request.TastingId);
        if (registerData == null)
            return Result<MessageResponse>.Failure("Tasting not found.", 404);

        if (registerData.Deadline < DateTime.UtcNow)
            return Result<MessageResponse>.Failure("Registration deadline has passed.", 400);

        if (registerData.Attendees.Count >= registerData.Capacity)
            return Result<MessageResponse>.Failure("Tasting is already at full capacity.", 400);

        var isRegistered = await attendeeRepository.IsUserRegisteredAsync(userId, request.TastingId);
        if (isRegistered)
            return Result<MessageResponse>.Failure("You are already registered for this tasting.", 400);

        var attendee = new Attendee
        {
            UserId = userId,
            TastingId = request.TastingId,
            RegisteredAt = DateTime.UtcNow
        };

        await attendeeRepository.AddAsync(attendee);
        await attendeeRepository.SaveChangesAsync();

        return Result<MessageResponse>.Success(new MessageResponse("Successfully registered for the tasting."));
    }

    public async Task<Result<TastingResponse>> GetTastingByIdAsync(Guid tastingId)
    {
        var tasting = await tastingRepository.GetByIdAsync(tastingId);
        if (tasting == null)
            return Result<TastingResponse>.Failure("Tasting not found.", 404);

        var response = tastingMapper.ToTastingResponse(tasting);
        return Result<TastingResponse>.Success(response);
    }

    public async Task<Result<List<TastingResponse>>> GetUpcomingTastingsAsync()
    {
        var tastings = await tastingRepository.GetUpcomingTastingsAsync();
        var response = tastingMapper.ToTastingResponse(tastings);
        return Result<List<TastingResponse>>.Success(response);
    }
}