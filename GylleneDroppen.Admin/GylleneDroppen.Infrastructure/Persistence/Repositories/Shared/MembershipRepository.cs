using GylleneDroppen.Application.Interfaces.Shared.Repositories;
using GylleneDroppen.Domain.Entities;
using GylleneDroppen.Domain.Enums;
using GylleneDroppen.Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace GylleneDroppen.Infrastructure.Persistence.Repositories.Shared;

public class MembershipRepository(AppDbContext context) : Repository<Membership>(context), IMembershipRepository
{
    public async Task<Membership?> GetByUserIdAsync(Guid userId)
    {
        return await DbSet.FirstOrDefaultAsync(m => m.UserId == userId);
    }


    public async Task<bool> HasActiveFreeTrial(Guid userId)
    {
        return await DbSet.AnyAsync(m =>
            m.UserId == userId &&
            m.Type == MembershipType.FreeTrial &&
            m.Status == MembershipStatus.Active);
    }

    public async Task<List<Membership>> GetExpiredMembershipsAsync()
    {
        return await DbSet.Where(m =>
                m.Status == MembershipStatus.Active &&
                m.EndDate != null &&
                m.EndDate < DateTime.UtcNow)
            .ToListAsync();
    }

    public async Task<int> CountMembershipsByTypeAsync(MembershipType type)
    {
        return await DbSet.CountAsync(m => m.Type == type);
    }
}