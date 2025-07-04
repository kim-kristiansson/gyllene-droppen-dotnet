using GylleneDroppen.Application.Dtos.WhiskyMetadata;

namespace GylleneDroppen.Application.Interfaces.Services;

public interface IWhiskyMetadataService
{
    // Whisky Types
    Task<List<WhiskyTypeDto>> GetAllWhiskyTypesAsync();
    Task<WhiskyTypeDto?> GetWhiskyTypeByIdAsync(Guid id);
    Task<WhiskyTypeDto> CreateWhiskyTypeAsync(CreateWhiskyTypeRequestDto request);
    Task<WhiskyTypeDto> UpdateWhiskyTypeAsync(Guid id, UpdateWhiskyTypeRequestDto request);
    Task<bool> DeleteWhiskyTypeAsync(Guid id);

    // Countries
    Task<List<CountryDto>> GetAllCountriesAsync();
    Task<CountryDto?> GetCountryByIdAsync(Guid id);
    Task<CountryDto> CreateCountryAsync(CreateCountryRequestDto request);
    Task<CountryDto> UpdateCountryAsync(Guid id, UpdateCountryRequestDto request);
    Task<bool> DeleteCountryAsync(Guid id);

    // Regions
    Task<List<RegionDto>> GetAllRegionsAsync();
    Task<List<RegionDto>> GetRegionsByCountryAsync(Guid countryId);
    Task<RegionDto?> GetRegionByIdAsync(Guid id);
    Task<RegionDto> CreateRegionAsync(CreateRegionRequestDto request);
    Task<RegionDto> UpdateRegionAsync(Guid id, UpdateRegionRequestDto request);
    Task<bool> DeleteRegionAsync(Guid id);
}