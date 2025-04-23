using GylleneDroppen.Application.Common.Results;
using GylleneDroppen.Application.Dtos.Common;
using GylleneDroppen.Application.Dtos.Tasting;
using GylleneDroppen.Application.Interfaces.Admin.Mappers;
using GylleneDroppen.Application.Interfaces.Public.Services;
using GylleneDroppen.Application.Interfaces.Shared.Repositories;
using GylleneDroppen.Domain.Entities;

namespace GylleneDroppen.Application.Services.Public;

public class TastingService(
    ITastingRepository tastingRepository,
    IAttendeeRepository attendeeRepository,
    IAdminTastingMapper tastingMapper)
    : ITastingService
{
    public async Task<Result<List<TastingUserResponse>>> GetUpcomingTastingsAsync()
    {
        var tastings = await tastingRepository.GetUpcomingTastingsAsync();
        var response = tastingMapper.ToTastingUserResponse(tastings);
        return Result<List<TastingUserResponse>>.Success(response);
    }

    public async Task<Result<TastingUserResponse>> GetTastingByIdAsync(Guid tastingId)
    {
        var tasting = await tastingRepository.GetByIdAsync(tastingId);
        if (tasting == null)
            return Result<TastingUserResponse>.Failure("Tasting not found.", 404);

        var response = tastingMapper.ToTastingUserResponse(tasting);
        return Result<TastingUserResponse>.Success(response);
    }

    public async Task<Result<MessageResponse>> RegisterForTastingAsync(Guid userId, RegisterForTastingRequest request)
    {
        var registerData = await tastingRepository.GetRegisterTastingDataAsync(request.TastingId);
        if (registerData == null)
            return Result<MessageResponse>.Failure("Tasting not found.", 404);

        if (registerData.Deadline < DateTime.UtcNow)
            return Result<MessageResponse>.Failure("Registration deadline has passed.", 400);

        if (registerData.ParticipantCount >= registerData.Capacity)
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
}