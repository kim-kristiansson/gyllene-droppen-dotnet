using GylleneDroppen.Application.Dtos.Whisky;
using GylleneDroppen.Application.Interfaces.Repositories;
using GylleneDroppen.Application.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace GylleneDroppen.Application.Services;

public class WhiskyService(
    IWhiskyRepository whiskyRepository,
    ITastingHistoryRepository tastingHistoryRepository,
    ICurrentUserService currentUserService,
    ILogger<WhiskyService> logger) : IWhiskyService
{
    public async Task<WhiskySearchResultDto> SearchWhiskiesAsync(WhiskySearchDto searchDto)
    {
        return await whiskyRepository.SearchWhiskiesAsync(searchDto);
    }

    public async Task<WhiskyResponseDto?> GetWhiskyByIdAsync(Guid id)
    {
        var whisky = await whiskyRepository.GetWhiskyWithDetailsAsync(id);
        return whisky == null ? null : MapToResponseDto(whisky);
    }

    public async Task<WhiskyResponseDto> CreateWhiskyAsync(CreateWhiskyRequestDto dto, string? userId = null)
    {
        var currentUserId = userId ?? currentUserService.GetUserId();
        if (string.IsNullOrEmpty(currentUserId))
            throw new UnauthorizedAccessException("Användare måste vara inloggad för att skapa whiskies.");

        // Check if whisky with same name and distillery already exists
        if (await whiskyRepository.ExistsByNameAndDistilleryAsync(dto.Name, dto.Distillery))
        {
            throw new InvalidOperationException(
                $"En whisky med namnet '{dto.Name}' från '{dto.Distillery}' finns redan.");
        }

        var whisky = new Whisky
        {
            Id = Guid.NewGuid(),
            Name = dto.Name,
            Distillery = dto.Distillery,
            Age = dto.Age,
            Abv = dto.Abv,
            Region = dto.Region,
            Type = dto.Type,
            Country = dto.Country,
            Color = dto.Color,
            Nose = dto.Nose,
            Palate = dto.Palate,
            Finish = dto.Finish,
            Price = dto.Price,
            BottleSize = dto.BottleSize,
            CreatedDate = DateTime.UtcNow,
            CreatedByUserId = currentUserId
        };

        await whiskyRepository.AddAsync(whisky);
        await whiskyRepository.SaveChangesAsync();

        logger.LogInformation("Whisky '{WhiskyName}' created by user {UserId}", dto.Name, currentUserId);

        return MapToResponseDto(whisky);
    }

    public async Task<WhiskyResponseDto> UpdateWhiskyAsync(UpdateWhiskyRequestDto dto, string? userId = null)
    {
        var currentUserId = userId ?? currentUserService.GetUserId();
        if (string.IsNullOrEmpty(currentUserId))
            throw new UnauthorizedAccessException("Användare måste vara inloggad för att uppdatera whiskies.");

        var whisky = await whiskyRepository.GetByIdAsync(dto.Id);
        if (whisky == null)
            throw new InvalidOperationException("Whiskyn hittades inte.");

        // Check if another whisky with same name and distillery already exists
        if (await whiskyRepository.ExistsByNameAndDistilleryAsync(dto.Name, dto.Distillery, dto.Id))
        {
            throw new InvalidOperationException(
                $"En annan whisky med namnet '{dto.Name}' från '{dto.Distillery}' finns redan.");
        }

        whisky.Name = dto.Name;
        whisky.Distillery = dto.Distillery;
        whisky.Age = dto.Age;
        whisky.Abv = dto.Abv;
        whisky.Region = dto.Region;
        whisky.Type = dto.Type;
        whisky.Country = dto.Country;
        whisky.Color = dto.Color;
        whisky.Nose = dto.Nose;
        whisky.Palate = dto.Palate;
        whisky.Finish = dto.Finish;
        whisky.Price = dto.Price;
        whisky.BottleSize = dto.BottleSize;
        whisky.UpdatedDate = DateTime.UtcNow;
        whisky.UpdatedByUserId = currentUserId;

        whiskyRepository.Update(whisky);
        await whiskyRepository.SaveChangesAsync();

        logger.LogInformation("Whisky '{WhiskyName}' updated by user {UserId}", dto.Name, currentUserId);

        return MapToResponseDto(whisky);
    }

    public async Task<bool> DeleteWhiskyAsync(Guid id, string? userId = null)
    {
        var currentUserId = userId ?? currentUserService.GetUserId();
        if (string.IsNullOrEmpty(currentUserId))
            throw new UnauthorizedAccessException("Användare måste vara inloggad för att ta bort whiskies.");

        var whisky = await whiskyRepository.GetByIdAsync(id);
        if (whisky == null)
            return false;

        // Check if whisky has tasting history
        var tastingHistory = await tastingHistoryRepository.GetTastingHistoryByWhiskyIdAsync(id);
        if (tastingHistory.Any())
        {
            throw new InvalidOperationException(
                "Kan inte radera whisky som har provningshistorik. Ta bort provningshistoriken först.");
        }

        whiskyRepository.Remove(whisky);
        await whiskyRepository.SaveChangesAsync();

        logger.LogInformation("Whisky '{WhiskyName}' deleted by user {UserId}", whisky.Name, currentUserId);

        return true;
    }

    public async Task<bool> WhiskyExistsAsync(Guid id)
    {
        return await whiskyRepository.GetByIdAsync(id) != null;
    }

    public async Task<List<WhiskyResponseDto>> GetFeaturedWhiskiesAsync(int count = 6)
    {
        var whiskies = await whiskyRepository.GetAllAsync();
        return whiskies
            .OrderByDescending(w => w.CreatedDate)
            .Take(count)
            .Select(MapToResponseDto)
            .ToList();
    }

    public async Task<bool> UpdateWhiskyImageAsync(Guid id, string imagePath, string? userId = null)
    {
        var currentUserId = userId ?? currentUserService.GetUserId();
        if (string.IsNullOrEmpty(currentUserId))
            throw new UnauthorizedAccessException("Användare måste vara inloggad för att uppdatera whisky-bilder.");

        var whisky = await whiskyRepository.GetByIdAsync(id);
        if (whisky == null)
            return false;

        whisky.ImagePath = imagePath;
        whisky.UpdatedDate = DateTime.UtcNow;
        whisky.UpdatedByUserId = currentUserId;

        whiskyRepository.Update(whisky);
        await whiskyRepository.SaveChangesAsync();

        logger.LogInformation("Whisky '{WhiskyName}' image updated by user {UserId}", whisky.Name, currentUserId);

        return true;
    }

    private static WhiskyResponseDto MapToResponseDto(Whisky whisky)
    {
        return new WhiskyResponseDto
        {
            Id = whisky.Id,
            Name = whisky.Name,
            Distillery = whisky.Distillery,
            Age = whisky.Age,
            Abv = whisky.Abv,
            Region = whisky.Region,
            Type = whisky.Type,
            Country = whisky.Country,
            Color = whisky.Color,
            Nose = whisky.Nose,
            Palate = whisky.Palate,
            Finish = whisky.Finish,
            Price = whisky.Price,
            BottleSize = whisky.BottleSize,
            ImagePath = whisky.ImagePath,
            CreatedDate = whisky.CreatedDate,
            UpdatedDate = whisky.UpdatedDate,
            CreatedByUserName = whisky.CreatedByUser?.Email ?? "Unknown",
            UpdatedByUserName = whisky.UpdatedByUser?.Email,
            TastingCount = whisky.TastingHistories?.Count ?? 0
        };
    }
}