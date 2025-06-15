using GylleneDroppen.Application.Interfaces.Repositories;
using GylleneDroppen.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace GylleneDroppen.Infrastructure.Persistence.Repositories;

public class TastingEventWhiskyRepository(ApplicationDbContext context)
    : Repository<TastingEventWhisky>(context), ITastingEventWhiskyRepository
{
    public async Task<TastingEventWhisky?> GetByEventAndWhiskyAsync(Guid eventId, Guid whiskyId)
    {
        return await DbSet
            .Include(tew => tew.Whisky)
            .Include(tew => tew.AddedByUser)
            .FirstOrDefaultAsync(tew => tew.TastingEventId == eventId && tew.WhiskyId == whiskyId);
    }

    public async Task<List<TastingEventWhisky>> GetByEventIdAsync(Guid eventId)
    {
        return await DbSet
            .Include(tew => tew.Whisky)
            .Include(tew => tew.AddedByUser)
            .Where(tew => tew.TastingEventId == eventId)
            .OrderBy(tew => tew.Order)
            .ToListAsync();
    }

    public async Task<bool> ExistsAsync(Guid eventId, Guid whiskyId)
    {
        return await DbSet.AnyAsync(tew => tew.TastingEventId == eventId && tew.WhiskyId == whiskyId);
    }

    public async Task UpdateOrdersAsync(Guid eventId, List<(Guid WhiskyId, int Order)> orders)
    {
        var eventWhiskies = await DbSet
            .Where(tew => tew.TastingEventId == eventId)
            .ToListAsync();

        foreach (var order in orders)
        {
            var eventWhisky = eventWhiskies.FirstOrDefault(ew => ew.WhiskyId == order.WhiskyId);
            if (eventWhisky != null)
            {
                eventWhisky.Order = order.Order;
            }
        }
    }
}