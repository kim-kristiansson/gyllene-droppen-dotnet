namespace GylleneDroppen.Application.Interfaces.Repositories.Shared;

public class AttendeeRepository(AppDbContext context) : Repository<Attendee>(context), IAttendeeRepository
{
    public async Task<bool> IsUserRegisteredAsync(Guid userId, Guid eventId)
    {
        return await DbSet
            .AnyAsync(p => p.UserId == userId && p.EventId == eventId);
    }
}