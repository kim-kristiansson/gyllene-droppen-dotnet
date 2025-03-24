using GylleneDroppen.Core.Entities;
using GylleneDroppen.Core.Interfaces.Repositories;
using GylleneDroppen.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GylleneDroppen.Infrastructure.Repositories;

public class AttendeeRepository(AppDbContext context) : Repository<Attendee>(context), IAttendeeRepository
{
    public async Task<bool> IsUserRegisteredAsync(Guid userId, Guid eventId)
    {
        return await DbSet
            .AnyAsync(a => a.UserId == userId && a.EventId == eventId);
    }
}