using GylleneDroppen.Application.Dtos.Membership;
using GylleneDroppen.Application.Interfaces.Repositories;
using GylleneDroppen.Application.Interfaces.Services;
using GylleneDroppen.Core.Entities;
using Microsoft.Extensions.Logging;

namespace GylleneDroppen.Application.Services;

public class MembershipService(
    IMembershipPeriodRepository membershipPeriodRepository,
    IUserMembershipRepository userMembershipRepository,
    IUserTrialUsageRepository userTrialUsageRepository,
    IUserRepository userRepository,
    ICurrentUserService currentUserService,
    ILogger<MembershipService> logger) : IMembershipService
{
    public async Task<List<MembershipPeriodDto>> GetAllMembershipPeriodsAsync()
    {
        var periods = await membershipPeriodRepository.GetAllAsync();
        return periods.Select(MapToDto).ToList();
    }

    public async Task<List<MembershipPeriodDto>> GetActiveMembershipPeriodsAsync()
    {
        var periods = await membershipPeriodRepository.GetActivePeriodsAsync();
        return periods.Select(MapToDto).ToList();
    }

    public async Task<MembershipPeriodDto?> GetMembershipPeriodByIdAsync(Guid id)
    {
        var period = await membershipPeriodRepository.GetByIdAsync(id);
        return period == null ? null : MapToDto(period);
    }

    public async Task<MembershipPeriodDto> CreateMembershipPeriodAsync(CreateMembershipPeriodRequestDto dto)
    {
        var currentUserId = currentUserService.GetUserId();
        if (string.IsNullOrEmpty(currentUserId))
            throw new UnauthorizedAccessException("Användare måste vara inloggad.");

        var period = new MembershipPeriod
        {
            Id = Guid.NewGuid(),
            Name = dto.Name,
            DurationInMonths = dto.DurationInMonths,
            Price = dto.Price,
            IsActive = dto.IsActive,
            CreatedDate = DateTime.UtcNow,
            CreatedByUserId = currentUserId
        };

        await membershipPeriodRepository.AddAsync(period);
        await membershipPeriodRepository.SaveChangesAsync();

        logger.LogInformation("Membership period '{PeriodName}' created by user {UserId}", dto.Name, currentUserId);

        var createdPeriod = await membershipPeriodRepository.GetByIdWithDetailsAsync(period.Id);
        return MapToDto(createdPeriod!);
    }

    public async Task<MembershipPeriodDto> UpdateMembershipPeriodAsync(Guid id, CreateMembershipPeriodRequestDto dto)
    {
        var currentUserId = currentUserService.GetUserId();
        if (string.IsNullOrEmpty(currentUserId))
            throw new UnauthorizedAccessException("Användare måste vara inloggad.");

        var period = await membershipPeriodRepository.GetByIdAsync(id);
        if (period == null)
            throw new InvalidOperationException("Medlemskapsperioden hittades inte.");

        period.Name = dto.Name;
        period.DurationInMonths = dto.DurationInMonths;
        period.Price = dto.Price;
        period.IsActive = dto.IsActive;
        period.UpdatedDate = DateTime.UtcNow;
        period.UpdatedByUserId = currentUserId;

        membershipPeriodRepository.Update(period);
        await membershipPeriodRepository.SaveChangesAsync();

        logger.LogInformation("Membership period '{PeriodId}' updated by user {UserId}", id, currentUserId);

        var updatedPeriod = await membershipPeriodRepository.GetByIdWithDetailsAsync(period.Id);
        return MapToDto(updatedPeriod!);
    }

    public async Task<bool> DeleteMembershipPeriodAsync(Guid id)
    {
        var period = await membershipPeriodRepository.GetByIdAsync(id);
        if (period == null)
            return false;

        // Check if any active memberships use this period
        var activeMemberships = await userMembershipRepository.GetActiveMembershipsByPeriodAsync(id);
        if (activeMemberships.Any())
            throw new InvalidOperationException("Kan inte ta bort medlemskapsperiod som används av aktiva medlemskap.");

        membershipPeriodRepository.Remove(period);
        await membershipPeriodRepository.SaveChangesAsync();

        logger.LogInformation("Membership period {PeriodId} deleted", id);
        return true;
    }

    public async Task<List<UserMembershipDto>> GetUserMembershipsAsync(string userId)
    {
        var memberships = await userMembershipRepository.GetUserMembershipsAsync(userId);
        return memberships.Select(MapToDto).ToList();
    }

    public async Task<UserMembershipDto?> GetCurrentUserMembershipAsync(string userId)
    {
        var membership = await userMembershipRepository.GetCurrentUserMembershipAsync(userId);
        return membership == null ? null : MapToDto(membership);
    }

    public async Task<UserMembershipDto> CreateUserMembershipAsync(CreateUserMembershipRequestDto dto)
    {
        var currentUserId = currentUserService.GetUserId();
        if (string.IsNullOrEmpty(currentUserId))
            throw new UnauthorizedAccessException("Användare måste vara inloggad.");

        var period = await membershipPeriodRepository.GetByIdAsync(dto.MembershipPeriodId);
        if (period == null)
            throw new InvalidOperationException("Medlemskapsperioden hittades inte.");

        var user = await userRepository.GetByIdAsync(dto.UserId);
        if (user == null)
            throw new InvalidOperationException("Användaren hittades inte.");

        var endDate = dto.StartDate.AddMonths(period.DurationInMonths);

        var membership = new UserMembership
        {
            Id = Guid.NewGuid(),
            UserId = dto.UserId,
            MembershipPeriodId = dto.MembershipPeriodId,
            StartDate = dto.StartDate.Kind == DateTimeKind.Utc ? dto.StartDate : dto.StartDate.ToUniversalTime(),
            EndDate = endDate.Kind == DateTimeKind.Utc ? endDate : endDate.ToUniversalTime(),
            AmountPaid = dto.AmountPaid,
            IsActive = dto.IsActive,
            Notes = dto.Notes,
            CreatedDate = DateTime.UtcNow,
            CreatedByUserId = currentUserId
        };

        await userMembershipRepository.AddAsync(membership);
        await userMembershipRepository.SaveChangesAsync();

        logger.LogInformation("Membership created for user {UserId} with period {PeriodId}", dto.UserId, dto.MembershipPeriodId);

        var createdMembership = await userMembershipRepository.GetByIdWithDetailsAsync(membership.Id);
        return MapToDto(createdMembership!);
    }

    public async Task<bool> ExtendUserMembershipAsync(string userId, Guid membershipPeriodId, decimal amountPaid, string? notes = null)
    {
        var currentMembership = await userMembershipRepository.GetCurrentUserMembershipAsync(userId);
        var period = await membershipPeriodRepository.GetByIdAsync(membershipPeriodId);
        
        if (period == null)
            throw new InvalidOperationException("Medlemskapsperioden hittades inte.");

        DateTime startDate;
        if (currentMembership != null && currentMembership.IsCurrentlyValid)
        {
            // Extend from current end date
            startDate = currentMembership.EndDate;
        }
        else
        {
            // Start new membership from today
            startDate = DateTime.UtcNow;
        }

        var dto = new CreateUserMembershipRequestDto
        {
            UserId = userId,
            MembershipPeriodId = membershipPeriodId,
            StartDate = startDate,
            AmountPaid = amountPaid,
            Notes = notes
        };

        await CreateUserMembershipAsync(dto);
        return true;
    }

    public async Task<bool> DeactivateUserMembershipAsync(Guid membershipId)
    {
        var membership = await userMembershipRepository.GetByIdAsync(membershipId);
        if (membership == null)
            return false;

        membership.IsActive = false;
        userMembershipRepository.Update(membership);
        await userMembershipRepository.SaveChangesAsync();

        logger.LogInformation("Membership {MembershipId} deactivated", membershipId);
        return true;
    }

    public async Task<bool> HasUserUsedTrialAsync(string userId)
    {
        var trial = await userTrialUsageRepository.GetByUserIdAsync(userId);
        return trial?.HasUsedTrial ?? false;
    }

    public async Task<bool> HasEmailUsedTrialAsync(string email)
    {
        return await userTrialUsageRepository.HasEmailUsedTrialAsync(email);
    }

    public async Task CreateTrialForUserAsync(string userId, string email)
    {
        var existingTrial = await userTrialUsageRepository.GetByUserIdAsync(userId);
        if (existingTrial != null)
            return; // Trial already exists

        var emailUsed = await HasEmailUsedTrialAsync(email);
        if (emailUsed)
            throw new InvalidOperationException("Denna e-post har redan använt sitt gratisprov.");

        var trial = new UserTrialUsage
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Email = email.ToLowerInvariant(),
            HasUsedTrial = false,
            CreatedDate = DateTime.UtcNow
        };

        await userTrialUsageRepository.AddAsync(trial);
        await userTrialUsageRepository.SaveChangesAsync();

        logger.LogInformation("Trial created for user {UserId} with email {Email}", userId, email);
    }

    public async Task<bool> UseTrialForEventAsync(string userId, Guid eventId)
    {
        var trial = await userTrialUsageRepository.GetByUserIdAsync(userId);
        if (trial == null || trial.HasUsedTrial)
            return false;

        trial.HasUsedTrial = true;
        trial.TrialUsedDate = DateTime.UtcNow;
        trial.TrialUsedForEventId = eventId;
        trial.UpdatedDate = DateTime.UtcNow;

        userTrialUsageRepository.Update(trial);
        await userTrialUsageRepository.SaveChangesAsync();

        logger.LogInformation("Trial used by user {UserId} for event {EventId}", userId, eventId);
        return true;
    }

    public async Task<UserMembershipStatusDto> GetUserMembershipStatusAsync(string userId)
    {
        var currentMembership = await GetCurrentUserMembershipAsync(userId);
        var hasUsedTrial = await HasUserUsedTrialAsync(userId);
        var trial = await userTrialUsageRepository.GetByUserIdAsync(userId);

        var hasActiveMembership = currentMembership?.IsCurrentlyValid ?? false;
        var hasTrialAvailable = trial != null && !hasUsedTrial;
        var canRegister = hasActiveMembership || hasTrialAvailable;

        string statusMessage;
        string badgeText;
        string badgeClass;
        DateTime? membershipEndDate = null;

        if (hasActiveMembership && currentMembership != null)
        {
            var daysRemaining = (currentMembership.EndDate - DateTime.UtcNow).Days;
            statusMessage = $"Medlem till {currentMembership.EndDate:yyyy-MM-dd}";
            badgeText = $"Medlem ({daysRemaining} dagar kvar)";
            badgeClass = daysRemaining <= 7 ? "badge-warning" : "badge-success";
            membershipEndDate = currentMembership.EndDate;
        }
        else if (hasTrialAvailable)
        {
            statusMessage = "Gratisprov tillgängligt";
            badgeText = "Gratisprov";
            badgeClass = "badge-info";
        }
        else if (hasUsedTrial)
        {
            statusMessage = "Gratisprov använt - medlemskap krävs";
            badgeText = "Prov använt";
            badgeClass = "badge-secondary";
        }
        else
        {
            statusMessage = "Medlemskap krävs";
            badgeText = "Ej medlem";
            badgeClass = "badge-danger";
        }

        return new UserMembershipStatusDto
        {
            HasActiveMembership = hasActiveMembership,
            HasUsedTrial = hasUsedTrial,
            HasTrialAvailable = hasTrialAvailable,
            CanRegisterForEvents = canRegister,
            MembershipEndDate = membershipEndDate,
            CurrentMembership = currentMembership,
            StatusMessage = statusMessage,
            BadgeText = badgeText,
            BadgeClass = badgeClass
        };
    }

    public async Task<bool> CanUserRegisterForEventsAsync(string userId)
    {
        // First check if user has a trial record, create one if missing
        var user = await userRepository.GetByIdAsync(userId);
        if (user != null && !string.IsNullOrEmpty(user.Email))
        {
            await EnsureUserHasTrialAsync(userId, user.Email);
        }
        
        var status = await GetUserMembershipStatusAsync(userId);
        return status.CanRegisterForEvents;
    }

    public async Task EnsureUserHasTrialAsync(string userId, string email)
    {
        var existingTrial = await userTrialUsageRepository.GetByUserIdAsync(userId);
        if (existingTrial == null)
        {
            // User doesn't have a trial record - create one
            await CreateTrialForUserAsync(userId, email);
        }
    }

    public async Task<List<UserMembershipDto>> GetAllActiveMembershipsAsync()
    {
        var memberships = await userMembershipRepository.GetAllActiveMembershipsAsync();
        return memberships.Select(MapToDto).ToList();
    }

    public async Task<List<UserMembershipDto>> GetExpiringMembershipsAsync(int daysAhead = 30)
    {
        var memberships = await userMembershipRepository.GetExpiringMembershipsAsync(daysAhead);
        return memberships.Select(MapToDto).ToList();
    }

    public async Task<int> GetTrialUsageCountAsync()
    {
        return await userTrialUsageRepository.GetTrialUsageCountAsync();
    }

    private static MembershipPeriodDto MapToDto(MembershipPeriod period)
    {
        return new MembershipPeriodDto
        {
            Id = period.Id,
            Name = period.Name,
            DurationInMonths = period.DurationInMonths,
            Price = period.Price,
            IsActive = period.IsActive,
            CreatedDate = period.CreatedDate,
            CreatedByUserName = period.CreatedByUser?.Email ?? "Okänd"
        };
    }

    private static UserMembershipDto MapToDto(UserMembership membership)
    {
        var daysRemaining = (membership.EndDate - DateTime.UtcNow).Days;
        return new UserMembershipDto
        {
            Id = membership.Id,
            UserId = membership.UserId,
            UserName = $"{membership.User?.FirstName} {membership.User?.LastName}".Trim(),
            UserEmail = membership.User?.Email ?? "Okänd",
            MembershipPeriodId = membership.MembershipPeriodId,
            MembershipPeriodName = membership.MembershipPeriod?.Name ?? "Okänd",
            DurationInMonths = membership.MembershipPeriod?.DurationInMonths ?? 0,
            StartDate = membership.StartDate,
            EndDate = membership.EndDate,
            AmountPaid = membership.AmountPaid,
            IsActive = membership.IsActive,
            IsExpired = membership.IsExpired,
            IsCurrentlyValid = membership.IsCurrentlyValid,
            Notes = membership.Notes,
            CreatedDate = membership.CreatedDate,
            CreatedByUserName = membership.CreatedByUser?.Email ?? "Okänd",
            DaysRemaining = daysRemaining > 0 ? daysRemaining : 0
        };
    }
}