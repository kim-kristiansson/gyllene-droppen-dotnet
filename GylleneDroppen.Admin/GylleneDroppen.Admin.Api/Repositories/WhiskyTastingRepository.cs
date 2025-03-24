using GylleneDroppen.Admin.Api.Interfaces.Repositories;
using GylleneDroppen.Admin.Api.Queries.WhiskyTasting;
using GylleneDroppen.Core.Entities;
using GylleneDroppen.Infrastructure.Data;
using GylleneDroppen.Infrastructure.Queries.Generic;
using GylleneDroppen.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace GylleneDroppen.Admin.Api.Repositories;

public class WhiskyTastingRepository(AppDbContext context)
    : Repository<WhiskyTasting>(context), IWhiskyTastingRepository
{
    public async Task<List<UpcomingWhiskyTastingQuery>> GetUpcomingWhiskyTastingAsync()
    {
        return await DbSet
            .Where(wt => wt.StartTime > DateTime.Now)
            .Select(wt => new UpcomingWhiskyTastingQuery
            {
                Id = wt.Id,
                Title = wt.Title,
                Description = wt.Description,
                StartTime = wt.StartTime,
                EndTime = wt.EndTime,
                Location = wt.Location,
                Capacity = wt.Capacity,
                Price = wt.Price,
                Deadline = wt.Deadline,
                Organizer = wt.Organizer != null
                    ? new OrganizerQuery
                    {
                        Id = wt.OrganizerId,
                        Name = $"{wt.Organizer.FirstName} {wt.Organizer.LastName}"
                    }
                    : null
            })
            .Where(wt => wt.Organizer != null)
            .OrderBy(wt => wt.StartTime)
            .ToListAsync();
    }

    public async Task<RegisterForWhiskyTastingQuery?> GetRegisterWhiskyTastingDataAsync(Guid eventId)
    {
        return await DbSet
            .Where(wt => wt.Id == eventId)
            .Select(wt => new RegisterForWhiskyTastingQuery
            {
                Id = wt.Id,
                Deadline = wt.Deadline,
                Capacity = wt.Capacity,
                AttendeeCount = wt.Attendees.Count
            })
            .FirstOrDefaultAsync();
    }
}