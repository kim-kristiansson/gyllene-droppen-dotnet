using GylleneDroppen.Api.Data;
using GylleneDroppen.Api.Models;
using GylleneDroppen.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GylleneDroppen.Api.Repositories;

public class EventRepository(AppDbContext context) : Repository<Event>(context), IEventRepository
{
    public async Task<List<Event>> GetUpcomingEventsAsync()
    {
        return await DbSet
            .Where(e => e.StartTime > DateTime.Now)
            .OrderBy(e => e.StartTime)
            .ToListAsync();
    }
}