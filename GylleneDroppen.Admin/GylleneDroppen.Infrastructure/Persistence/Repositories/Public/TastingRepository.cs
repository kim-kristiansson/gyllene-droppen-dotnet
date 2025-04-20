using GylleneDroppen.Application.Interfaces.Shared.Repositories;
using GylleneDroppen.Application.Queries;
using GylleneDroppen.Domain.Entities;
using GylleneDroppen.Infrastructure.Persistence.Data;
using GylleneDroppen.Infrastructure.Persistence.Repositories.Shared;
using Microsoft.EntityFrameworkCore;

namespace GylleneDroppen.Infrastructure.Persistence.Repositories.Public;

public class TastingRepository(AppDbContext context) : Repository<Tasting>(context), ITastingRepository
{
    public async Task<List<UpcomingTastingsQuery>> GetUpcomingTastingsAsync()
    {
        return await DbSet
            .Where(e => e.StartTime > DateTime.Now)
            .Select(e => new UpcomingTastingsQuery
            {
                Id = e.Id,
                Title = e.Title,
                Description = e.Description,
                StartTime = e.StartTime,
                EndTime = e.EndTime,
                Location = e.Location,
                Capacity = e.Capacity,
                Price = e.Price,
                Deadline = e.Deadline,
                Organizer = e.Organizer != null
                    ? new OrganizerQuery
                    {
                        Id = e.OrganizerId,
                        Name = $"{e.Organizer.FirstName} {e.Organizer.LastName}"
                    }
                    : null
            })
            .Where(e => e.Organizer != null)
            .OrderBy(e => e.StartTime)
            .ToListAsync();
    }

    public async Task<RegisterTastingQuery?> GetRegisterTastingDataAsync(Guid eventId)
    {
        return await DbSet
            .Where(e => e.Id == eventId)
            .Select(e => new RegisterTastingQuery
            {
                Id = e.Id,
                Deadline = e.Deadline,
                Capacity = e.Capacity,
                ParticipantCount = e.Attendees.Count
            })
            .FirstOrDefaultAsync();
    }
}