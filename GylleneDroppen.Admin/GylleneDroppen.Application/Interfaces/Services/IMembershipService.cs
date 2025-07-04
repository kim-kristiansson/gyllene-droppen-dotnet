using GylleneDroppen.Application.Dtos.Membership;

namespace GylleneDroppen.Application.Interfaces.Services;

public interface IMembershipService
{
    // Membership Period Management
    Task<List<MembershipPeriodDto>> GetAllMembershipPeriodsAsync();
    Task<List<MembershipPeriodDto>> GetActiveMembershipPeriodsAsync();
    Task<MembershipPeriodDto?> GetMembershipPeriodByIdAsync(Guid id);
    Task<MembershipPeriodDto> CreateMembershipPeriodAsync(CreateMembershipPeriodRequestDto dto);
    Task<MembershipPeriodDto> UpdateMembershipPeriodAsync(Guid id, CreateMembershipPeriodRequestDto dto);
    Task<bool> DeleteMembershipPeriodAsync(Guid id);

    // User Membership Management
    Task<List<UserMembershipDto>> GetUserMembershipsAsync(string userId);
    Task<UserMembershipDto?> GetCurrentUserMembershipAsync(string userId);
    Task<UserMembershipDto> CreateUserMembershipAsync(CreateUserMembershipRequestDto dto);
    Task<bool> ExtendUserMembershipAsync(string userId, Guid membershipPeriodId, decimal amountPaid, string? notes = null);
    Task<bool> DeactivateUserMembershipAsync(Guid membershipId);

    // Trial Management
    Task<bool> HasUserUsedTrialAsync(string userId);
    Task<bool> HasEmailUsedTrialAsync(string email);
    Task CreateTrialForUserAsync(string userId, string email);
    Task<bool> UseTrialForEventAsync(string userId, Guid eventId);
    Task EnsureUserHasTrialAsync(string userId, string email);

    // Membership Status
    Task<UserMembershipStatusDto> GetUserMembershipStatusAsync(string userId);
    Task<bool> CanUserRegisterForEventsAsync(string userId);

    // Admin Functions
    Task<List<UserMembershipDto>> GetAllActiveMembershipsAsync();
    Task<List<UserMembershipDto>> GetExpiringMembershipsAsync(int daysAhead = 30);
    Task<int> GetTrialUsageCountAsync();
}