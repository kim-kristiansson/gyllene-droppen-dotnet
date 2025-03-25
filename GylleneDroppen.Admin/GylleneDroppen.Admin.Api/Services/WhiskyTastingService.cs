using GylleneDroppen.Admin.Api.Dtos.WhiskyTasting;
using GylleneDroppen.Admin.Api.Interfaces.Mappers;
using GylleneDroppen.Admin.Api.Interfaces.Repositories;
using GylleneDroppen.Admin.Api.Interfaces.Services;
using GylleneDroppen.Core.Common;
using GylleneDroppen.Core.Dtos.Generic;

namespace GylleneDroppen.Admin.Api.Services;

public class WhiskyTastingService(
    IWhiskyTastingRepository whiskyTastingRepository,
    IWhiskyTastingMapper whiskyTastingMapper)
    : IWhiskyTastingService
{
    public async Task<Result<MessageResponse>> CreateWhiskyTastingAsync(CreateWhiskyTastingRequest request)
    {
        if (request.EndTime <= request.StartTime)
            return Result<MessageResponse>.Failure("End time must be after start time.", 400);

        if (request.Deadline >= request.StartTime)
            return Result<MessageResponse>.Failure("Deadline must be before whisky tasting start time.", 400);

        var newWhiskyTasting = whiskyTastingMapper.ToWhiskyTasting(request);

        await whiskyTastingRepository.AddAsync(newWhiskyTasting);
        await whiskyTastingRepository.SaveChangesAsync();

        return Result<MessageResponse>.Success(
            new MessageResponse("WhiskyTastingAdminResponse created successfully."));
    }

    public async Task<Result<List<WhiskyTastingAdminResponse>>> GetUpcomingWhiskyTastingsAsync()
    {
        var queries = await whiskyTastingRepository.GetUpcomingWhiskyTastingAsync();

        var response = whiskyTastingMapper.ToWhiskyTastingResponse(queries);

        return Result<List<WhiskyTastingAdminResponse>>.Success(response);
    }

    public async Task<Result<List<WhiskyTastingAdminResponse>>> GetAllWhiskyTastingsAsync()
    {
        var whiskyTastings = await whiskyTastingRepository.GetAllAsync();

        var response = whiskyTastingMapper.ToWhiskyTastingResponse(whiskyTastings);

        return Result<List<WhiskyTastingAdminResponse>>.Success(response);
    }

    public async Task<Result<MessageResponse>> UpdateWhiskyTastingAsync(UpdateWhiskyTastingRequest request)
    {
        var existingWhiskyTasting = await whiskyTastingRepository.GetByIdAsync(request.Id);
        if (existingWhiskyTasting is null)
            return Result<MessageResponse>.Failure("whisky tasting not found.", 404);

        if (request.EndTime <= request.StartTime)
            return Result<MessageResponse>.Failure("End time must be after start time.", 400);

        if (request.Deadline >= request.StartTime)
            return Result<MessageResponse>.Failure("Deadline must be before whisky tasting start time.", 400);

        if (existingWhiskyTasting.Title != request.Title)
            existingWhiskyTasting.Title = request.Title;

        if (existingWhiskyTasting.Description != request.Description)
            existingWhiskyTasting.Description = request.Description;

        if (existingWhiskyTasting.Location != request.Location)
            existingWhiskyTasting.Location = request.Location;

        if (existingWhiskyTasting.StartTime != request.StartTime)
            existingWhiskyTasting.StartTime = request.StartTime;

        if (existingWhiskyTasting.EndTime != request.EndTime)
            existingWhiskyTasting.EndTime = request.EndTime;

        if (existingWhiskyTasting.Capacity != request.Capacity)
            existingWhiskyTasting.Capacity = request.Capacity;

        if (existingWhiskyTasting.Price != request.Price)
            existingWhiskyTasting.Price = request.Price;

        if (existingWhiskyTasting.Deadline != request.Deadline)
            existingWhiskyTasting.Deadline = request.Deadline;

        if (existingWhiskyTasting.OrganizerId != request.OrganizerId)
            existingWhiskyTasting.OrganizerId = request.OrganizerId;

        whiskyTastingRepository.Update(existingWhiskyTasting);
        await whiskyTastingRepository.SaveChangesAsync();

        return Result<MessageResponse>.Success(new MessageResponse("Successfully updated whisky tasting."));
    }
}