using GylleneDroppen.Application.Interfaces.Repositories;
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

    public async Task<int> CountUserAttendancesAsync(Guid userId)
    {
        return await DbSet.CountAsync(a => a.UserId == userId);
    }
}