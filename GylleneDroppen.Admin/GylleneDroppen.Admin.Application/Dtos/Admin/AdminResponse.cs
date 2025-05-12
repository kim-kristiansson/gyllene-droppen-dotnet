namespace GylleneDroppen.Admin.Application.Dtos.Admin;

public class AdminResponse
{
    public required Guid Id { get; set; }
    public required string Email { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
}