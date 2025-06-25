using GylleneDroppen.Core.Entities;

namespace GylleneDroppen.Application.Interfaces.Repositories;

public interface IUserMembershipRepository : IRepository<UserMembership>
{
    Task<List<UserMembership>> GetUserMembershipsAsync(string userId);
    Task<UserMembership?> GetCurrentUserMembershipAsync(string userId);
    Task<UserMembership?> GetByIdWithDetailsAsync(Guid id);
    Task<List<UserMembership>> GetActiveMembershipsByPeriodAsync(Guid periodId);
    Task<List<UserMembership>> GetAllActiveMembershipsAsync();
    Task<List<UserMembership>> GetExpiringMembershipsAsync(int daysAhead);
}