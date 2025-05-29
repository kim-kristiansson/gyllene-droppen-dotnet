using GylleneDroppen.Application.Dtos.Whisky;

namespace GylleneDroppen.Application.Interfaces.Services;

public interface IWhiskyService
{
    Task<WhiskySearchResultDto> SearchWhiskiesAsync(WhiskySearchDto searchDto);
    Task<WhiskyResponseDto?> GetWhiskyByIdAsync(Guid id);
    Task<WhiskyResponseDto> CreateWhiskyAsync(CreateWhiskyRequestDto dto, string userId);
    Task<WhiskyResponseDto> UpdateWhiskyAsync(UpdateWhiskyRequestDto dto, string userId);
    Task<bool> DeleteWhiskyAsync(Guid id, string userId);
    Task<bool> WhiskyExistsAsync(Guid id);
    Task<List<WhiskyResponseDto>> GetFeaturedWhiskiesAsync(int count = 6);
    Task<bool> UpdateWhiskyImageAsync(Guid id, string imagePath, string userId);
}