namespace GylleneDroppen.Admin.Application.Dtos.User;

public class AdminUserDetailResponse
{
    public required Guid Id { get; set; }
    public required string Email { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Role { get; set; }
    public required DateTime CreatedAt { get; set; }
}