using GylleneDroppen.Domain.Entities;
using GylleneDroppen.Domain.Enums;

namespace GylleneDroppen.Application.Interfaces.Shared.Repositories;

public interface IMembershipRepository : IRepository<Membership>
{
    Task<Membership?> GetByUserIdAsync(Guid userId);
    Task<bool> HasActiveFreeTrial(Guid userId);
    Task<List<Membership>> GetExpiredMembershipsAsync();
    Task<int> CountMembershipsByTypeAsync(MembershipType type);
}