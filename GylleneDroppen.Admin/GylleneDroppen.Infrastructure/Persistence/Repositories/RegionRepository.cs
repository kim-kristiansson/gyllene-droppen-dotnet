using GylleneDroppen.Application.Interfaces.Repositories;
using GylleneDroppen.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace GylleneDroppen.Infrastructure.Persistence.Repositories;

public class RegionRepository : Repository<Region>, IRegionRepository
{
    public RegionRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<List<Region>> GetRegionsByCountryAsync(Guid countryId)
    {
        return await _context.Regions
            .Where(r => r.CountryId == countryId)
            .OrderBy(r => r.Name)
            .ToListAsync();
    }

    public async Task<bool> ExistsWithNameInCountryAsync(string name, Guid countryId, Guid? excludeId = null)
    {
        var query = _context.Regions
            .Where(r => r.Name.ToLower() == name.ToLower() && r.CountryId == countryId);
        
        if (excludeId.HasValue)
        {
            query = query.Where(r => r.Id != excludeId.Value);
        }
        
        return await query.AnyAsync();
    }
}