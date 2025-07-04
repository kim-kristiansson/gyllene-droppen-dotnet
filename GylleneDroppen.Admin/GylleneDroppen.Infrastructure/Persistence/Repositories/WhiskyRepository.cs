using GylleneDroppen.Application.Dtos.Whisky;
using GylleneDroppen.Application.Interfaces.Repositories;
using GylleneDroppen.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace GylleneDroppen.Infrastructure.Persistence.Repositories;

public class WhiskyRepository(ApplicationDbContext context)
    : Repository<Whisky>(context), IWhiskyRepository
{
    public async Task<WhiskySearchResultDto> SearchWhiskiesAsync(WhiskySearchDto searchDto)
    {
        var query = DbSet.Include(w => w.CreatedByUser)
            .Include(w => w.UpdatedByUser)
            .Include(w => w.TastingHistories)
            .Include(w => w.Region!)
                .ThenInclude(r => r.Country!)
            .Include(w => w.WhiskyType!)
                .ThenInclude(wt => wt.OriginCountry!)
            .AsQueryable();

        // Apply filters
        if (!string.IsNullOrEmpty(searchDto.SearchTerm))
        {
            var searchTerm = searchDto.SearchTerm.ToLower();
            query = query.Where(w => w.Name.ToLower().Contains(searchTerm) ||
                                     w.Distillery.ToLower().Contains(searchTerm) ||
                                     (w.Region != null && w.Region.Name.ToLower().Contains(searchTerm)));
        }

        if (!string.IsNullOrEmpty(searchDto.Country))
        {
            query = query.Where(w => 
                (w.Region != null && w.Region.Country != null && w.Region.Country.Name == searchDto.Country) ||
                (w.WhiskyType != null && w.WhiskyType.OriginCountry != null && w.WhiskyType.OriginCountry.Name == searchDto.Country));
        }

        if (!string.IsNullOrEmpty(searchDto.Region))
        {
            query = query.Where(w => w.Region != null && w.Region.Name == searchDto.Region);
        }

        if (!string.IsNullOrEmpty(searchDto.Type))
        {
            query = query.Where(w => w.WhiskyType != null && w.WhiskyType.Name == searchDto.Type);
        }

        if (searchDto.MinAge.HasValue)
        {
            query = query.Where(w => w.Age >= searchDto.MinAge.Value);
        }

        if (searchDto.MaxAge.HasValue)
        {
            query = query.Where(w => w.Age <= searchDto.MaxAge.Value);
        }

        if (searchDto.MinAbv.HasValue)
        {
            query = query.Where(w => w.Abv >= searchDto.MinAbv.Value);
        }

        if (searchDto.MaxAbv.HasValue)
        {
            query = query.Where(w => w.Abv <= searchDto.MaxAbv.Value);
        }

        if (searchDto.MinPrice.HasValue)
        {
            query = query.Where(w => w.Price >= searchDto.MinPrice.Value);
        }

        if (searchDto.MaxPrice.HasValue)
        {
            query = query.Where(w => w.Price <= searchDto.MaxPrice.Value);
        }

        // Apply sorting
        query = searchDto.SortBy?.ToLower() switch
        {
            "age" => searchDto.SortDescending
                ? query.OrderByDescending(w => w.Age)
                : query.OrderBy(w => w.Age),
            "abv" => searchDto.SortDescending
                ? query.OrderByDescending(w => w.Abv)
                : query.OrderBy(w => w.Abv),
            "price" => searchDto.SortDescending
                ? query.OrderByDescending(w => w.Price)
                : query.OrderBy(w => w.Price),
            "createddate" => searchDto.SortDescending
                ? query.OrderByDescending(w => w.CreatedDate)
                : query.OrderBy(w => w.CreatedDate),
            _ => searchDto.SortDescending
                ? query.OrderByDescending(w => w.Name)
                : query.OrderBy(w => w.Name)
        };

        var totalCount = await query.CountAsync();

        var whiskies = await query
            .Skip((searchDto.Page - 1) * searchDto.PageSize)
            .Take(searchDto.PageSize)
            .ToListAsync();

        var whiskyDtos = whiskies.Select(w => new WhiskyResponseDto
        {
            Id = w.Id,
            Name = w.Name,
            Distillery = w.Distillery,
            Age = w.Age,
            Abv = w.Abv,
            RegionId = w.RegionId,
            Region = w.Region?.Name ?? "Okänd",
            WhiskyTypeId = w.WhiskyTypeId,
            Type = w.WhiskyType?.Name ?? "Okänd",
            Country = w.Region?.Country?.Name ?? w.WhiskyType?.OriginCountry?.Name ?? "Okänd",
            Color = w.Color,
            Nose = w.Nose,
            Palate = w.Palate,
            Finish = w.Finish,
            Price = w.Price,
            BottleSize = w.BottleSize,
            ImagePath = w.ImagePath,
            CreatedDate = w.CreatedDate,
            UpdatedDate = w.UpdatedDate,
            CreatedByUserName = w.CreatedByUser?.Email ?? "Okänd",
            UpdatedByUserName = w.UpdatedByUser?.Email,
            TastingCount = w.TastingHistories.Count
        }).ToList();

        return new WhiskySearchResultDto
        {
            Whiskies = whiskyDtos,
            TotalCount = totalCount,
            Page = searchDto.Page,
            PageSize = searchDto.PageSize
        };
    }

    public async Task<Whisky?> GetWhiskyWithDetailsAsync(Guid id)
    {
        return await DbSet
            .Include(w => w.CreatedByUser)
            .Include(w => w.UpdatedByUser)
            .Include(w => w.Region!)
                .ThenInclude(r => r.Country!)
            .Include(w => w.WhiskyType!)
                .ThenInclude(wt => wt.OriginCountry!)
            .Include(w => w.TastingHistories)
            .ThenInclude(th => th.OrganizedByUser)
            .FirstOrDefaultAsync(w => w.Id == id);
    }

    public async Task<List<Whisky>> GetWhiskiesByIdsAsync(List<Guid> ids)
    {
        return await DbSet
            .Include(w => w.CreatedByUser)
            .Include(w => w.TastingHistories)
            .Where(w => ids.Contains(w.Id))
            .ToListAsync();
    }

    public async Task<bool> ExistsByNameAndDistilleryAsync(string name, string distillery, Guid? excludeId = null)
    {
        var query = DbSet.Where(w => w.Name == name && w.Distillery == distillery);

        if (excludeId.HasValue)
        {
            query = query.Where(w => w.Id != excludeId.Value);
        }

        return await query.AnyAsync();
    }
}