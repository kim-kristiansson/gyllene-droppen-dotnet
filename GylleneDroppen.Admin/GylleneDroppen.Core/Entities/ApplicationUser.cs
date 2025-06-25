using Microsoft.AspNetCore.Identity;

namespace GylleneDroppen.Core.Entities;

public class ApplicationUser : IdentityUser
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }

    // Membership navigation properties
    public ICollection<UserMembership> Memberships { get; set; } = new List<UserMembership>();
    public UserTrialUsage? TrialUsage { get; set; }

    // Helper methods for membership status
    public bool HasActiveMembership => Memberships.Any(m => m.IsCurrentlyValid);
    public bool HasUsedTrial => TrialUsage?.HasUsedTrial ?? false;
    public bool CanRegisterForEvents => HasActiveMembership || (!HasUsedTrial && TrialUsage != null);
    
    public UserMembership? CurrentMembership => Memberships
        .Where(m => m.IsCurrentlyValid)
        .OrderByDescending(m => m.EndDate)
        .FirstOrDefault();
}