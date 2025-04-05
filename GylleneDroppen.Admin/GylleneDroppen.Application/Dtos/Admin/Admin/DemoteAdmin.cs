namespace GylleneDroppen.Application.Dtos.Admin.Admin;

public class DemoteAdminRequest
{
    public required Guid UserId { get; set; }
    public required Guid AdminId { get; set; }
}