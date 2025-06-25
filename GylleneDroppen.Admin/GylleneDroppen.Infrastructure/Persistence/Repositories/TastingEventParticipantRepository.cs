using GylleneDroppen.Application.Interfaces.Repositories;
using GylleneDroppen.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace GylleneDroppen.Infrastructure.Persistence.Repositories;

public class TastingEventParticipantRepository(ApplicationDbContext context)
    : Repository<TastingEventParticipant>(context), ITastingEventParticipantRepository
{
    public async Task<TastingEventParticipant?> GetByEventAndUserAsync(Guid eventId, string userId)
    {
        return await DbSet
            .Include(p => p.User)
            .Include(p => p.TastingEvent)
            .FirstOrDefaultAsync(p => p.TastingEventId == eventId && p.UserId == userId);
    }

    public async Task<List<TastingEventParticipant>> GetByEventIdAsync(Guid eventId)
    {
        return await DbSet
            .Include(p => p.User)
            .Where(p => p.TastingEventId == eventId)
            .OrderBy(p => p.RegisteredDate)
            .ToListAsync();
    }

    public async Task<List<TastingEventParticipant>> GetByUserIdAsync(string userId)
    {
        return await DbSet
            .Include(p => p.TastingEvent)
            .Where(p => p.UserId == userId)
            .OrderByDescending(p => p.TastingEvent.EventDate)
            .ToListAsync();
    }

    public async Task<bool> IsUserRegisteredAsync(Guid eventId, string userId)
    {
        return await DbSet.AnyAsync(p => p.TastingEventId == eventId && p.UserId == userId);
    }

    public async Task<int> GetParticipantCountAsync(Guid eventId)
    {
        return await DbSet.CountAsync(p => p.TastingEventId == eventId);
    }
}