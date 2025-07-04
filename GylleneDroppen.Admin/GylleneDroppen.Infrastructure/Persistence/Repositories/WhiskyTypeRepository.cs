using GylleneDroppen.Application.Interfaces.Repositories;
using GylleneDroppen.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace GylleneDroppen.Infrastructure.Persistence.Repositories;

public class WhiskyTypeRepository : Repository<WhiskyType>, IWhiskyTypeRepository
{
    public WhiskyTypeRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<bool> ExistsWithNameAsync(string name, Guid? excludeId = null)
    {
        var query = _context.WhiskyTypes.Where(wt => wt.Name.ToLower() == name.ToLower());
        
        if (excludeId.HasValue)
        {
            query = query.Where(wt => wt.Id != excludeId.Value);
        }
        
        return await query.AnyAsync();
    }

    public async Task<List<WhiskyType>> GetAllWithOriginsAsync()
    {
        return await _context.WhiskyTypes
            .Include(wt => wt.CreatedByUser)
            .Include(wt => wt.OriginCountry)
            .Include(wt => wt.OriginRegion)
            .OrderBy(wt => wt.Name)
            .ToListAsync();
    }
}