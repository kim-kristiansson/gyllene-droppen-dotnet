using GylleneDroppen.Application.Interfaces.Shared.Repositories;
using GylleneDroppen.Domain.Entities;
using GylleneDroppen.Infrastructure.Persistence.Data;
using GylleneDroppen.Infrastructure.Persistence.Repositories.Shared;
using Microsoft.EntityFrameworkCore;

namespace GylleneDroppen.Infrastructure.Persistence.Repositories.Public;

public class TastingRepository(AppDbContext context) : Repository<Tasting>(context), ITastingRepository
{
    public async Task<List<Tasting>> GetUpcomingTastingsAsync()
    {
        return await DbSet
            .Where(e => e.StartTime > DateTime.Now)
            .Where(e => e.Organizer != null)
            .OrderBy(e => e.StartTime)
            .ToListAsync();
    }

    public async Task<Tasting?> GetRegisterTastingDataAsync(Guid eventId)
    {
        return await DbSet
            .Where(e => e.Id == eventId)
            .FirstOrDefaultAsync();
    }
}