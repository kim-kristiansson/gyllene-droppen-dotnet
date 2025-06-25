namespace GylleneDroppen.Application.Dtos.Membership;

public class UserMembershipStatusDto
{
    public bool HasActiveMembership { get; set; }
    public bool HasUsedTrial { get; set; }
    public bool CanRegisterForEvents { get; set; }
    public UserMembershipDto? CurrentMembership { get; set; }
    public string StatusMessage { get; set; } = string.Empty;
    public string BadgeText { get; set; } = string.Empty;
    public string BadgeClass { get; set; } = string.Empty; // CSS class for styling
}