using GylleneDroppen.Application.Dtos.Tasting;
using GylleneDroppen.Application.Dtos.Whisky;

namespace GylleneDroppen.Application.Interfaces.Services;

public interface ITastingHistoryService
{
    Task<List<TastingHistoryDto>> GetTastingHistoryByWhiskyIdAsync(Guid whiskyId);
    Task<List<TastingHistoryDto>> GetRecentTastingHistoryAsync(int count = 10);
    Task<List<TastingHistoryDto>> GetTastingHistoryByUserAsync(string userId);
    Task<TastingHistoryDto> CreateTastingHistoryAsync(CreateTastingHistoryRequestDto dto, string userId);
    Task<bool> DeleteTastingHistoryAsync(Guid id, string userId);
}