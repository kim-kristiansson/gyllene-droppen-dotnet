namespace GylleneDroppen.Admin.Api.Models;

public class User
{
    public Guid Id { get; init; }
    public string Email { get; init; } = string.Empty;
    public string PasswordHash { get; init; } = string.Empty;
}