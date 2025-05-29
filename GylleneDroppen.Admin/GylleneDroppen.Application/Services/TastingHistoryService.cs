using GylleneDroppen.Application.Dtos.Tasting;
using GylleneDroppen.Application.Interfaces.Repositories;
using GylleneDroppen.Application.Interfaces.Services;
using GylleneDroppen.Core.Entities;
using Microsoft.Extensions.Logging;

namespace GylleneDroppen.Application.Services;

public class TastingHistoryService(
    ITastingHistoryRepository tastingHistoryRepository,
    IWhiskyRepository whiskyRepository,
    ICurrentUserService currentUserService,
    ILogger<TastingHistoryService> logger) : ITastingHistoryService
{
    public async Task<List<TastingHistoryDto>> GetTastingHistoryByWhiskyIdAsync(Guid whiskyId)
    {
        var histories = await tastingHistoryRepository.GetTastingHistoryByWhiskyIdAsync(whiskyId);
        return histories.Select(MapToDto).ToList();
    }

    public async Task<List<TastingHistoryDto>> GetRecentTastingHistoryAsync(int count = 10)
    {
        var histories = await tastingHistoryRepository.GetRecentTastingHistoryAsync(count);
        return histories.Select(MapToDto).ToList();
    }

    public async Task<List<TastingHistoryDto>> GetTastingHistoryByUserAsync(string userId)
    {
        var histories = await tastingHistoryRepository.GetTastingHistoryByUserAsync(userId);
        return histories.Select(MapToDto).ToList();
    }

    public async Task<TastingHistoryDto> CreateTastingHistoryAsync(CreateTastingHistoryRequestDto dto)
    {
        var currentUserId = currentUserService.GetUserId();
        if (string.IsNullOrEmpty(currentUserId))
            throw new UnauthorizedAccessException("Användare måste vara inloggad för att skapa provningshistorik.");

        // Verify whisky exists
        var whisky = await whiskyRepository.GetByIdAsync(dto.WhiskyId);
        if (whisky == null)
            throw new InvalidOperationException("Whiskyn hittades inte.");

        var tastingHistory = new TastingHistory
        {
            Id = Guid.NewGuid(),
            WhiskyId = dto.WhiskyId,
            EventTitle = dto.EventTitle,
            TastingDate = dto.TastingDate,
            Notes = dto.Notes,
            OrganizedByUserId = currentUserId,
            CreatedDate = DateTime.UtcNow
        };

        await tastingHistoryRepository.AddAsync(tastingHistory);
        await tastingHistoryRepository.SaveChangesAsync();

        logger.LogInformation("Tasting history created for whisky {WhiskyId} by user {UserId}", dto.WhiskyId,
            currentUserId);

        return MapToDto(tastingHistory);
    }

    public async Task<bool> DeleteTastingHistoryAsync(Guid id)
    {
        var currentUserId = currentUserService.GetUserId();
        if (string.IsNullOrEmpty(currentUserId))
            throw new UnauthorizedAccessException("Användare måste vara inloggad för att ta bort provningshistorik.");

        var tastingHistory = await tastingHistoryRepository.GetByIdAsync(id);
        if (tastingHistory == null)
            return false;

        tastingHistoryRepository.Remove(tastingHistory);
        await tastingHistoryRepository.SaveChangesAsync();

        logger.LogInformation("Tasting history {TastingHistoryId} deleted by user {UserId}", id, currentUserId);

        return true;
    }

    private static TastingHistoryDto MapToDto(TastingHistory tastingHistory)
    {
        return new TastingHistoryDto
        {
            Id = tastingHistory.Id,
            WhiskyId = tastingHistory.WhiskyId,
            WhiskyName = tastingHistory.Whisky?.Name ?? "Unknown",
            EventTitle = tastingHistory.EventTitle,
            TastingDate = tastingHistory.TastingDate,
            Notes = tastingHistory.Notes,
            OrganizedByUserName = tastingHistory.OrganizedByUser?.Email ?? "Unknown",
            CreatedDate = tastingHistory.CreatedDate
        };
    }
}