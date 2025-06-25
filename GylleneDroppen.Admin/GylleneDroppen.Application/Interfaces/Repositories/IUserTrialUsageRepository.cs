using GylleneDroppen.Core.Entities;

namespace GylleneDroppen.Application.Interfaces.Repositories;

public interface IUserTrialUsageRepository : IRepository<UserTrialUsage>
{
    Task<UserTrialUsage?> GetByUserIdAsync(string userId);
    Task<bool> HasEmailUsedTrialAsync(string email);
    Task<int> GetTrialUsageCountAsync();
}