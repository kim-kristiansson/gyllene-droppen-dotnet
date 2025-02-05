namespace GylleneDroppen.Admin.Api.Models;

public class User
{
    public required Guid Id { get; init; }
    public required string Email { get; init; }
    public required string PasswordHash { get; init; }
    public required string PasswordSalt { get; init; }
}