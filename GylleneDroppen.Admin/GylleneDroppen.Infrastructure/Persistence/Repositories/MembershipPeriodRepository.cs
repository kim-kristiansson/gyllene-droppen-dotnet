using GylleneDroppen.Application.Interfaces.Repositories;
using GylleneDroppen.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace GylleneDroppen.Infrastructure.Persistence.Repositories;

public class MembershipPeriodRepository(ApplicationDbContext context)
    : Repository<MembershipPeriod>(context), IMembershipPeriodRepository
{
    public async Task<List<MembershipPeriod>> GetActivePeriodsAsync()
    {
        return await DbSet
            .Where(mp => mp.IsActive)
            .OrderBy(mp => mp.DurationInMonths)
            .ToListAsync();
    }

    public async Task<MembershipPeriod?> GetByIdWithDetailsAsync(Guid id)
    {
        return await DbSet
            .Include(mp => mp.CreatedByUser)
            .Include(mp => mp.UpdatedByUser)
            .FirstOrDefaultAsync(mp => mp.Id == id);
    }
}