using GylleneDroppen.Core.Entities;

namespace GylleneDroppen.Application.Interfaces.Repositories;

public interface IMembershipPeriodRepository : IRepository<MembershipPeriod>
{
    Task<List<MembershipPeriod>> GetActivePeriodsAsync();
    Task<MembershipPeriod?> GetByIdWithDetailsAsync(Guid id);
}