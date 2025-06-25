using GylleneDroppen.Application.Interfaces.Repositories;
using GylleneDroppen.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace GylleneDroppen.Infrastructure.Persistence.Repositories;

public class UserMembershipRepository(ApplicationDbContext context)
    : Repository<UserMembership>(context), IUserMembershipRepository
{
    public async Task<List<UserMembership>> GetUserMembershipsAsync(string userId)
    {
        return await DbSet
            .Include(um => um.User)
            .Include(um => um.MembershipPeriod)
            .Include(um => um.CreatedByUser)
            .Where(um => um.UserId == userId)
            .OrderByDescending(um => um.CreatedDate)
            .ToListAsync();
    }

    public async Task<UserMembership?> GetCurrentUserMembershipAsync(string userId)
    {
        return await DbSet
            .Include(um => um.User)
            .Include(um => um.MembershipPeriod)
            .Include(um => um.CreatedByUser)
            .Where(um => um.UserId == userId && um.IsActive)
            .Where(um => um.StartDate <= DateTime.UtcNow && um.EndDate > DateTime.UtcNow)
            .OrderByDescending(um => um.EndDate)
            .FirstOrDefaultAsync();
    }

    public async Task<UserMembership?> GetByIdWithDetailsAsync(Guid id)
    {
        return await DbSet
            .Include(um => um.User)
            .Include(um => um.MembershipPeriod)
            .Include(um => um.CreatedByUser)
            .FirstOrDefaultAsync(um => um.Id == id);
    }

    public async Task<List<UserMembership>> GetActiveMembershipsByPeriodAsync(Guid periodId)
    {
        return await DbSet
            .Where(um => um.MembershipPeriodId == periodId && um.IsActive)
            .Where(um => um.EndDate > DateTime.UtcNow)
            .ToListAsync();
    }

    public async Task<List<UserMembership>> GetAllActiveMembershipsAsync()
    {
        return await DbSet
            .Include(um => um.User)
            .Include(um => um.MembershipPeriod)
            .Include(um => um.CreatedByUser)
            .Where(um => um.IsActive && um.EndDate > DateTime.UtcNow)
            .OrderBy(um => um.EndDate)
            .ToListAsync();
    }

    public async Task<List<UserMembership>> GetExpiringMembershipsAsync(int daysAhead)
    {
        var cutoffDate = DateTime.UtcNow.AddDays(daysAhead);
        
        return await DbSet
            .Include(um => um.User)
            .Include(um => um.MembershipPeriod)
            .Include(um => um.CreatedByUser)
            .Where(um => um.IsActive)
            .Where(um => um.EndDate > DateTime.UtcNow && um.EndDate <= cutoffDate)
            .OrderBy(um => um.EndDate)
            .ToListAsync();
    }
}