using GylleneDroppen.Application.Common.Results;
using GylleneDroppen.Application.Dtos.Common;
using GylleneDroppen.Application.Dtos.Tasting;
using GylleneDroppen.Application.Interfaces.Mappers.Admin;
using GylleneDroppen.Application.Interfaces.Repositories.Public;
using GylleneDroppen.Application.Interfaces.Repositories.Shared;
using GylleneDroppen.Application.Interfaces.Services.Admin;
using GylleneDroppen.Domain.Entities;

namespace GylleneDroppen.Application.Services.Public;

public class TastingService(
    ITastingRepository tastingRepository,
    ITastingMapper tastingMapper,
    IAttendeeRepository attendeeRepository)
    : ITastingService
{
    public async Task<Result<MessageResponse>> CreateTastingAsync(CreateTastingRequest request)
    {
        if (request.EndTime <= request.StartTime)
            return Result<MessageResponse>.Failure("End time must be after start time.", 400);

        if (request.Deadline >= request.StartTime)
            return Result<MessageResponse>.Failure("Deadline must be before tasting start time.", 400);

        var newTasting = tastingMapper.ToTasting(request);

        await tastingRepository.AddAsync(newTasting);
        await tastingRepository.SaveChangesAsync();

        return Result<MessageResponse>.Success(new MessageResponse("Tasting created successfully."));
    }

    public async Task<Result<List<TastingUserResponse>>> GetUpcomingTastingsAsync()
    {
        var queries = await tastingRepository.GetUpcomingTastingsAsync();

        var response = tastingMapper.ToTastingUserResponse(queries);

        return Result<List<TastingUserResponse>>.Success(response);
    }

    public async Task<Result<List<TastingAdminResponse>>> GetAllTastingsAsync()
    {
        var tastings = await tastingRepository.GetAllAsync();

        var response = tastingMapper.ToTastingAdminResponse(tastings);

        return Result<List<TastingAdminResponse>>.Success(response);
    }

    public async Task<Result<MessageResponse>> RegisterForTastingAsync(RegisterForTastingRequest request,
        Guid userId)
    {
        var tasting = await tastingRepository.GetRegisterTastingDataAsync(request.TastingId);

        if (tasting == null)
            return Result<MessageResponse>.Failure("Tasting not found.", 404);

        if (DateTime.UtcNow > tasting.Deadline)
            return Result<MessageResponse>.Failure("Registration deadline has passed.", 400);

        if (tasting.ParticipantCount >= tasting.Capacity)
            return Result<MessageResponse>.Failure("This tasting is fully booked.", 400);

        if (await attendeeRepository.IsUserRegisteredAsync(userId, request.TastingId))
            return Result<MessageResponse>.Failure("You are already registered for this tasting.", 400);

        var registration = new Attendee
        {
            UserId = userId,
            TastingId = request.TastingId,
            RegisteredAt = DateTime.UtcNow
        };

        await attendeeRepository.AddAsync(registration);

        return Result<MessageResponse>.Success(new MessageResponse("Successfully registered for the tasting."));
    }

    public async Task<Result<MessageResponse>> UpdateTastingAsync(UpdateTastingRequest tastingRequest)
    {
        var existingTasting = await tastingRepository.GetByIdAsync(tastingRequest.Id);
        if (existingTasting is null)
            return Result<MessageResponse>.Failure("Tasting not found.", 404);

        if (tastingRequest.EndTime <= tastingRequest.StartTime)
            return Result<MessageResponse>.Failure("End time must be after start time.", 400);

        if (tastingRequest.Deadline >= tastingRequest.StartTime)
            return Result<MessageResponse>.Failure("Deadline must be before tasting start time.", 400);

        if (existingTasting.Title != tastingRequest.Title)
            existingTasting.Title = tastingRequest.Title;

        if (existingTasting.Description != tastingRequest.Description)
            existingTasting.Description = tastingRequest.Description;

        if (existingTasting.Location != tastingRequest.Location)
            existingTasting.Location = tastingRequest.Location;

        if (existingTasting.StartTime != tastingRequest.StartTime)
            existingTasting.StartTime = tastingRequest.StartTime;

        if (existingTasting.EndTime != tastingRequest.EndTime)
            existingTasting.EndTime = tastingRequest.EndTime;

        if (existingTasting.Capacity != tastingRequest.Capacity)
            existingTasting.Capacity = tastingRequest.Capacity;

        if (existingTasting.Price != tastingRequest.Price)
            existingTasting.Price = tastingRequest.Price;

        if (existingTasting.Deadline != tastingRequest.Deadline)
            existingTasting.Deadline = tastingRequest.Deadline;

        if (existingTasting.OrganizerId != tastingRequest.OrganizerId)
            existingTasting.OrganizerId = tastingRequest.OrganizerId;

        tastingRepository.Update(existingTasting);
        await tastingRepository.SaveChangesAsync();

        return Result<MessageResponse>.Success(new MessageResponse("Successfully updated tasting."));
    }
}