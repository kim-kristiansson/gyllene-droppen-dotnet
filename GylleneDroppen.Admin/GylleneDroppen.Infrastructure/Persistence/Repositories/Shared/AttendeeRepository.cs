using GylleneDroppen.Application.Interfaces.Repositories.Shared;
using GylleneDroppen.Domain.Entities;
using GylleneDroppen.Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace GylleneDroppen.Infrastructure.Persistence.Repositories.Shared;

public class AttendeeRepository(AppDbContext context) : Repository<Attendee>(context), IAttendeeRepository
{
    public async Task<bool> IsUserRegisteredAsync(Guid userId, Guid eventId)
    {
        return await DbSet
            .AnyAsync(p => p.UserId == userId && p.TastingId == eventId);
    }
}