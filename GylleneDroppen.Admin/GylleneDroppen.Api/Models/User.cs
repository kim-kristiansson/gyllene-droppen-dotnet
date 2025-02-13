using GylleneDroppen.Api.Enums;

namespace GylleneDroppen.Api.Models;

public class User
{
    public required Guid Id { get; init; }
    public required string Email { get; init; }
    public required string PasswordHash { get; set; }
    public required string PasswordSalt { get; set; }
    public required RoleType Role { get; init; } 
}