using GylleneDroppen.Core.Domain.Enums;

namespace GylleneDroppen.Core.Domain.Entities;

public class User
{
    public required Guid Id { get; init; }
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public required string Email { get; init; }
    public required string PasswordHash { get; set; }
    public required string PasswordSalt { get; set; }
    public required RoleType Role { get; set; } = RoleType.User;
    public List<Attendee> Attendees { get; init; } = [];
}