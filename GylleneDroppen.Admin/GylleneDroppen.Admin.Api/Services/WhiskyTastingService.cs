using GylleneDroppen.Admin.Api.Dtos.WhiskyTasting;
using GylleneDroppen.Admin.Api.Interfaces.Mappers;
using GylleneDroppen.Admin.Api.Interfaces.Repositories;
using GylleneDroppen.Admin.Api.Interfaces.Services;
using GylleneDroppen.Shared.Dtos.Generic;
using GylleneDroppen.Shared.Utils;

namespace GylleneDroppen.Admin.Api.Services;

public class WhiskyTastingService(
    IWhiskyTastingRepository whiskyTastingRepository,
    IWhiskyTastingMapper whiskyTastingMapper)
    : IWhiskyTastingService
{
    public async Task<ServiceResponse<MessageResponse>> CreateWhiskyTastingAsync(CreateWhiskyTastingRequest request)
    {
        if (request.EndTime <= request.StartTime)
            return ServiceResponse<MessageResponse>.Failure("End time must be after start time.", 400);

        if (request.Deadline >= request.StartTime)
            return ServiceResponse<MessageResponse>.Failure("Deadline must be before whisky tasting start time.", 400);

        var newWhiskyTasting = whiskyTastingMapper.ToWhiskyTasting(request);

        await whiskyTastingRepository.AddAsync(newWhiskyTasting);
        await whiskyTastingRepository.SaveChangesAsync();

        return ServiceResponse<MessageResponse>.Success(
            new MessageResponse("WhiskyTastingResponse created successfully."));
    }

    public async Task<ServiceResponse<List<WhiskyTastingResponse>>> GetUpcomingWhiskyTastingsAsync()
    {
        var queries = await whiskyTastingRepository.GetUpcomingWhiskyTastingAsync();

        var response = whiskyTastingMapper.ToWhiskyTastingResponse(queries);

        return ServiceResponse<List<WhiskyTastingResponse>>.Success(response);
    }

    public async Task<ServiceResponse<List<WhiskyTastingResponse>>> GetAllWhiskyTastingsAsync()
    {
        var whiskyTastings = await whiskyTastingRepository.GetAllAsync();

        var response = whiskyTastingMapper.ToWhiskyTastingResponse(whiskyTastings);

        return ServiceResponse<List<WhiskyTastingResponse>>.Success(response);
    }

    public async Task<ServiceResponse<MessageResponse>> UpdateWhiskyTastingAsync(UpdateWhiskyTastingRequest request)
    {
        var existingWhiskyTasting = await whiskyTastingRepository.GetByIdAsync(request.Id);
        if (existingWhiskyTasting is null)
            return ServiceResponse<MessageResponse>.Failure("whisky tasting not found.", 404);

        if (request.EndTime <= request.StartTime)
            return ServiceResponse<MessageResponse>.Failure("End time must be after start time.", 400);

        if (request.Deadline >= request.StartTime)
            return ServiceResponse<MessageResponse>.Failure("Deadline must be before whisky tasting start time.", 400);

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

        return ServiceResponse<MessageResponse>.Success(new MessageResponse("Successfully updated whisky tasting."));
    }
}