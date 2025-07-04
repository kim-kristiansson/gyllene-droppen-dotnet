using GylleneDroppen.Application.Interfaces.Repositories;
using GylleneDroppen.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace GylleneDroppen.Infrastructure.Persistence.Repositories;

public class TastingEventRepository(ApplicationDbContext context)
    : Repository<TastingEvent>(context), ITastingEventRepository
{
    public async Task<TastingEvent?> GetTastingEventWithDetailsAsync(Guid id)
    {
        return await DbSet
            .Include(te => te.OrganizedByUser)
            .Include(te => te.Address)
            .ThenInclude(a => a!.CreatedByUser)
            .Include(te => te.TastingEventWhiskies)
            .ThenInclude(tew => tew.Whisky)
            .Include(te => te.TastingEventWhiskies)
            .ThenInclude(tew => tew.AddedByUser)
            .Include(te => te.Participants)
            .ThenInclude(p => p.User)
            .FirstOrDefaultAsync(te => te.Id == id);
    }

    public async Task<List<TastingEvent>> GetUpcomingTastingEventsAsync(int count = 10)
    {
        return await DbSet
            .Include(te => te.OrganizedByUser)
            .Include(te => te.Address)
            .Include(te => te.TastingEventWhiskies)
            .Include(te => te.Participants)
            .Where(te => te.EventDate >= DateTime.UtcNow)
            .OrderBy(te => te.EventDate)
            .Take(count)
            .ToListAsync();
    }

    public async Task<List<TastingEvent>> GetPastTastingEventsAsync(int count = 10)
    {
        return await DbSet
            .Include(te => te.OrganizedByUser)
            .Include(te => te.Address)
            .Include(te => te.TastingEventWhiskies)
            .Include(te => te.Participants)
            .Where(te => te.EventDate < DateTime.UtcNow)
            .OrderByDescending(te => te.EventDate)
            .Take(count)
            .ToListAsync();
    }

    public async Task<List<TastingEvent>> GetTastingEventsByOrganizerAsync(string userId)
    {
        return await DbSet
            .Include(te => te.OrganizedByUser)
            .Include(te => te.Address)
            .Include(te => te.TastingEventWhiskies)
            .Include(te => te.Participants)
            .Where(te => te.OrganizedByUserId == userId)
            .OrderByDescending(te => te.EventDate)
            .ToListAsync();
    }

    public async Task<List<TastingEvent>> GetPublicTastingEventsAsync()
    {
        return await DbSet
            .Include(te => te.OrganizedByUser)
            .Include(te => te.Address)
            .Include(te => te.TastingEventWhiskies)
            .Include(te => te.Participants)
            .Where(te => te.IsPublic)
            .OrderBy(te => te.EventDate)
            .ToListAsync();
    }
}