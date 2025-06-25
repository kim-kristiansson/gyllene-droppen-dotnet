using GylleneDroppen.Application.Interfaces.Repositories;
using GylleneDroppen.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace GylleneDroppen.Infrastructure.Persistence.Repositories;

public class UserTrialUsageRepository(ApplicationDbContext context)
    : Repository<UserTrialUsage>(context), IUserTrialUsageRepository
{
    public async Task<UserTrialUsage?> GetByUserIdAsync(string userId)
    {
        return await DbSet
            .Include(utu => utu.User)
            .FirstOrDefaultAsync(utu => utu.UserId == userId);
    }

    public async Task<bool> HasEmailUsedTrialAsync(string email)
    {
        return await DbSet
            .AnyAsync(utu => utu.Email.ToLower() == email.ToLower() && utu.HasUsedTrial);
    }

    public async Task<int> GetTrialUsageCountAsync()
    {
        return await DbSet
            .CountAsync(utu => utu.HasUsedTrial);
    }
}