using GylleneDroppen.Application.Interfaces.Repositories;
using GylleneDroppen.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace GylleneDroppen.Infrastructure.Persistence.Repositories;

public class TastingHistoryRepository(ApplicationDbContext context)
    : Repository<TastingHistory>(context), ITastingHistoryRepository
{
    public async Task<List<TastingHistory>> GetTastingHistoryByWhiskyIdAsync(Guid whiskyId)
    {
        return await DbSet
            .Include(th => th.Whisky)
            .Include(th => th.OrganizedByUser)
            .Where(th => th.WhiskyId == whiskyId)
            .OrderByDescending(th => th.TastingDate)
            .ToListAsync();
    }

    public async Task<List<TastingHistory>> GetRecentTastingHistoryAsync(int count = 10)
    {
        return await DbSet
            .Include(th => th.Whisky)
            .Include(th => th.OrganizedByUser)
            .OrderByDescending(th => th.TastingDate)
            .Take(count)
            .ToListAsync();
    }

    public async Task<List<TastingHistory>> GetTastingHistoryByUserAsync(string userId)
    {
        return await DbSet
            .Include(th => th.Whisky)
            .Include(th => th.OrganizedByUser)
            .Where(th => th.OrganizedByUserId == userId)
            .OrderByDescending(th => th.TastingDate)
            .ToListAsync();
    }
}