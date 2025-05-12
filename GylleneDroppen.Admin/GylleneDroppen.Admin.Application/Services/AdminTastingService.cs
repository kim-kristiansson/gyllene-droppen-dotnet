using GylleneDroppen.Admin.Application.Interfaces.Mappers;
using GylleneDroppen.Admin.Application.Interfaces.Services;
using GylleneDroppen.Application.Common.Results;
using GylleneDroppen.Application.Dtos.Common;
using GylleneDroppen.Application.Dtos.Tasting;
using GylleneDroppen.Application.Interfaces.Shared.Repositories;

namespace GylleneDroppen.Admin.Application.Services;

public class AdminTastingService(
    ITastingRepository tastingRepository,
    IAdminTastingMapper tastingMapper
) : IAdminTastingService
{
    public async Task<Result<List<TastingAdminResponse>>> GetAllUpcomingTastingsAsync()
    {
        var tastings = await tastingRepository.FindAsync(t => t.StartTime > DateTime.UtcNow);
        var response = tastingMapper.ToTastingAdminResponse(tastings);

        return Result<List<TastingAdminResponse>>.Success(response);
    }

    public async Task<Result<List<TastingAdminResponse>>> GetAllTastingsAsync()
    {
        var tastings = await tastingRepository.GetAllAsync();
        var response = tastingMapper.ToTastingAdminResponse(tastings);

        return Result<List<TastingAdminResponse>>.Success(response);
    }

    public async Task<Result<MessageResponse>> CreateTastingAsync(CreateTastingRequest request)
    {
        if (request.EndTime <= request.StartTime)
            return Result<MessageResponse>.Failure("Start time must be before end time.", 400);

        if (request.Deadline >= request.StartTime)
            return Result<MessageResponse>.Failure("Deadline must be before start time.", 400);

        var newTasting = tastingMapper.ToTasting(request);

        await tastingRepository.AddAsync(newTasting);
        await tastingRepository.SaveChangesAsync();

        return Result<MessageResponse>.Success(new MessageResponse("Tasting created successfully."));
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