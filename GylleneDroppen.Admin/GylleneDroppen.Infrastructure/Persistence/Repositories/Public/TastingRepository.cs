namespace GylleneDroppen.Application.Interfaces.Repositories.Public;

public class TastingRepository(AppDbContext context) : Repository<Event>(context), IEventRepository
{
    public async Task<List<EventUserResponse>> GetUpcomingEventsAsync()
    {
        return await DbSet
            .Where(e => e.StartTime > DateTime.Now)
            .Select(e => new EventUserResponse
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
                    ? new EventOrganizerResponse
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

    public async Task<RegisterEventQuery?> GetRegisterEventDataAsync(Guid eventId)
    {
        return await DbSet
            .Where(e => e.Id == eventId)
            .Select(e => new RegisterEventQuery
            {
                Id = e.Id,
                Deadline = e.Deadline,
                Capacity = e.Capacity,
                ParticipantCount = e.Participants.Count
            })
            .FirstOrDefaultAsync();
    }
}