using GylleneDroppen.Application.Dtos.WhiskyMetadata;
using GylleneDroppen.Application.Interfaces.Repositories;
using GylleneDroppen.Application.Interfaces.Services;
using GylleneDroppen.Core.Entities;

namespace GylleneDroppen.Application.Services;

public class WhiskyMetadataService : IWhiskyMetadataService
{
    private readonly IWhiskyTypeRepository _whiskyTypeRepository;
    private readonly ICountryRepository _countryRepository;
    private readonly IRegionRepository _regionRepository;
    private readonly ICurrentUserService _currentUserService;

    public WhiskyMetadataService(
        IWhiskyTypeRepository whiskyTypeRepository,
        ICountryRepository countryRepository,
        IRegionRepository regionRepository,
        ICurrentUserService currentUserService)
    {
        _whiskyTypeRepository = whiskyTypeRepository;
        _countryRepository = countryRepository;
        _regionRepository = regionRepository;
        _currentUserService = currentUserService;
    }

    #region Whisky Types

    public async Task<List<WhiskyTypeDto>> GetAllWhiskyTypesAsync()
    {
        var whiskyTypes = await _whiskyTypeRepository.GetAllWithOriginsAsync();
        return whiskyTypes.Select(MapToWhiskyTypeDto).ToList();
    }

    public async Task<WhiskyTypeDto?> GetWhiskyTypeByIdAsync(Guid id)
    {
        var whiskyType = await _whiskyTypeRepository.GetByIdAsync(id, wt => wt.CreatedByUser!, wt => wt.OriginCountry!, wt => wt.OriginRegion!);
        return whiskyType != null ? MapToWhiskyTypeDto(whiskyType) : null;
    }

    public async Task<WhiskyTypeDto> CreateWhiskyTypeAsync(CreateWhiskyTypeRequestDto request)
    {
        if (await _whiskyTypeRepository.ExistsWithNameAsync(request.Name))
        {
            throw new InvalidOperationException($"En whiskytyp med namnet '{request.Name}' finns redan.");
        }

        var whiskyType = new WhiskyType
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Description = request.Description,
            OriginCountryId = request.OriginCountryId,
            OriginRegionId = request.OriginRegionId,
            CreatedDate = DateTime.UtcNow,
            CreatedByUserId = _currentUserService.GetUserId() ?? throw new UnauthorizedAccessException()
        };

        await _whiskyTypeRepository.AddAsync(whiskyType);
        await _whiskyTypeRepository.SaveChangesAsync();

        return await GetWhiskyTypeByIdAsync(whiskyType.Id) ?? throw new InvalidOperationException();
    }

    public async Task<WhiskyTypeDto> UpdateWhiskyTypeAsync(Guid id, UpdateWhiskyTypeRequestDto request)
    {
        var whiskyType = await _whiskyTypeRepository.GetByIdAsync(id);
        if (whiskyType == null)
        {
            throw new ArgumentException($"Whiskytyp med ID {id} hittades inte.");
        }

        if (await _whiskyTypeRepository.ExistsWithNameAsync(request.Name, id))
        {
            throw new InvalidOperationException($"En whiskytyp med namnet '{request.Name}' finns redan.");
        }

        whiskyType.Name = request.Name;
        whiskyType.Description = request.Description;
        whiskyType.OriginCountryId = request.OriginCountryId;
        whiskyType.OriginRegionId = request.OriginRegionId;
        whiskyType.UpdatedDate = DateTime.UtcNow;
        whiskyType.UpdatedByUserId = _currentUserService.GetUserId();

        _whiskyTypeRepository.Update(whiskyType);
        await _whiskyTypeRepository.SaveChangesAsync();

        return await GetWhiskyTypeByIdAsync(id) ?? throw new InvalidOperationException();
    }

    public async Task<bool> DeleteWhiskyTypeAsync(Guid id)
    {
        var whiskyType = await _whiskyTypeRepository.GetByIdAsync(id);
        if (whiskyType == null) return false;

        _whiskyTypeRepository.Delete(whiskyType);
        await _whiskyTypeRepository.SaveChangesAsync();
        return true;
    }

    #endregion

    #region Countries

    public async Task<List<CountryDto>> GetAllCountriesAsync()
    {
        var countries = await _countryRepository.GetAllAsync(
            includeExpression: c => c.CreatedByUser,
            orderBy: c => c.OrderBy(x => x.Name));

        return countries.Select(MapToCountryDto).ToList();
    }

    public async Task<CountryDto?> GetCountryByIdAsync(Guid id)
    {
        var country = await _countryRepository.GetByIdAsync(id, c => c.CreatedByUser);
        return country != null ? MapToCountryDto(country) : null;
    }

    public async Task<CountryDto> CreateCountryAsync(CreateCountryRequestDto request)
    {
        if (await _countryRepository.ExistsWithNameAsync(request.Name))
        {
            throw new InvalidOperationException($"Ett land med namnet '{request.Name}' finns redan.");
        }

        var country = new Country
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            CreatedDate = DateTime.UtcNow,
            CreatedByUserId = _currentUserService.GetUserId() ?? throw new UnauthorizedAccessException()
        };

        await _countryRepository.AddAsync(country);
        await _countryRepository.SaveChangesAsync();

        return await GetCountryByIdAsync(country.Id) ?? throw new InvalidOperationException();
    }

    public async Task<CountryDto> UpdateCountryAsync(Guid id, UpdateCountryRequestDto request)
    {
        var country = await _countryRepository.GetByIdAsync(id);
        if (country == null)
        {
            throw new ArgumentException($"Land med ID {id} hittades inte.");
        }

        if (await _countryRepository.ExistsWithNameAsync(request.Name, id))
        {
            throw new InvalidOperationException($"Ett land med namnet '{request.Name}' finns redan.");
        }

        country.Name = request.Name;
        country.UpdatedDate = DateTime.UtcNow;
        country.UpdatedByUserId = _currentUserService.GetUserId();

        _countryRepository.Update(country);
        await _countryRepository.SaveChangesAsync();

        return await GetCountryByIdAsync(id) ?? throw new InvalidOperationException();
    }

    public async Task<bool> DeleteCountryAsync(Guid id)
    {
        var country = await _countryRepository.GetByIdAsync(id);
        if (country == null) return false;

        _countryRepository.Delete(country);
        await _countryRepository.SaveChangesAsync();
        return true;
    }

    #endregion

    #region Regions

    public async Task<List<RegionDto>> GetAllRegionsAsync()
    {
        var regions = await _regionRepository.GetAllAsync(
            includeExpression: r => r.CreatedByUser,
            orderBy: r => r.OrderBy(x => x.Name));

        return regions.Select(MapToRegionDto).ToList();
    }

    public async Task<List<RegionDto>> GetRegionsByCountryAsync(Guid countryId)
    {
        var regions = await _regionRepository.GetRegionsByCountryAsync(countryId);
        return regions.Select(MapToRegionDto).ToList();
    }

    public async Task<RegionDto?> GetRegionByIdAsync(Guid id)
    {
        var region = await _regionRepository.GetByIdAsync(id, r => r.CreatedByUser, r => r.Country);
        return region != null ? MapToRegionDto(region) : null;
    }

    public async Task<RegionDto> CreateRegionAsync(CreateRegionRequestDto request)
    {
        if (await _regionRepository.ExistsWithNameInCountryAsync(request.Name, request.CountryId))
        {
            throw new InvalidOperationException($"En region med namnet '{request.Name}' finns redan i detta land.");
        }

        var region = new Region
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Description = request.Description,
            CountryId = request.CountryId,
            CreatedDate = DateTime.UtcNow,
            CreatedByUserId = _currentUserService.GetUserId() ?? throw new UnauthorizedAccessException()
        };

        await _regionRepository.AddAsync(region);
        await _regionRepository.SaveChangesAsync();

        return await GetRegionByIdAsync(region.Id) ?? throw new InvalidOperationException();
    }

    public async Task<RegionDto> UpdateRegionAsync(Guid id, UpdateRegionRequestDto request)
    {
        var region = await _regionRepository.GetByIdAsync(id);
        if (region == null)
        {
            throw new ArgumentException($"Region med ID {id} hittades inte.");
        }

        if (await _regionRepository.ExistsWithNameInCountryAsync(request.Name, request.CountryId, id))
        {
            throw new InvalidOperationException($"En region med namnet '{request.Name}' finns redan i detta land.");
        }

        region.Name = request.Name;
        region.Description = request.Description;
        region.CountryId = request.CountryId;
        region.UpdatedDate = DateTime.UtcNow;
        region.UpdatedByUserId = _currentUserService.GetUserId();

        _regionRepository.Update(region);
        await _regionRepository.SaveChangesAsync();

        return await GetRegionByIdAsync(id) ?? throw new InvalidOperationException();
    }

    public async Task<bool> DeleteRegionAsync(Guid id)
    {
        var region = await _regionRepository.GetByIdAsync(id);
        if (region == null) return false;

        _regionRepository.Delete(region);
        await _regionRepository.SaveChangesAsync();
        return true;
    }

    #endregion

    #region Mapping Methods

    private static WhiskyTypeDto MapToWhiskyTypeDto(WhiskyType whiskyType)
    {
        return new WhiskyTypeDto
        {
            Id = whiskyType.Id,
            Name = whiskyType.Name,
            Description = whiskyType.Description,
            CreatedDate = whiskyType.CreatedDate,
            UpdatedDate = whiskyType.UpdatedDate,
            CreatedByUserName = whiskyType.CreatedByUser?.UserName ?? "Okänd",
            UpdatedByUserName = whiskyType.UpdatedByUser?.UserName,
            OriginCountryId = whiskyType.OriginCountryId,
            OriginCountryName = whiskyType.OriginCountry?.Name,
            OriginRegionId = whiskyType.OriginRegionId,
            OriginRegionName = whiskyType.OriginRegion?.Name
        };
    }

    private static CountryDto MapToCountryDto(Country country)
    {
        return new CountryDto
        {
            Id = country.Id,
            Name = country.Name,
            CreatedDate = country.CreatedDate,
            UpdatedDate = country.UpdatedDate,
            CreatedByUserName = country.CreatedByUser?.UserName ?? "Okänd",
            UpdatedByUserName = country.UpdatedByUser?.UserName,
            RegionCount = country.Regions?.Count ?? 0
        };
    }

    private static RegionDto MapToRegionDto(Region region)
    {
        return new RegionDto
        {
            Id = region.Id,
            Name = region.Name,
            Description = region.Description,
            CountryId = region.CountryId,
            CountryName = region.Country?.Name ?? "Okänd",
            CreatedDate = region.CreatedDate,
            UpdatedDate = region.UpdatedDate,
            CreatedByUserName = region.CreatedByUser?.UserName ?? "Okänd",
            UpdatedByUserName = region.UpdatedByUser?.UserName
        };
    }

    #endregion
}