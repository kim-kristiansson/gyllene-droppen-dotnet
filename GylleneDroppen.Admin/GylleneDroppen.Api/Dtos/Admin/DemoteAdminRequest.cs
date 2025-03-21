namespace GylleneDroppen.Api.Dtos.Admin;

public class DemoteAdminRequest
{
    public required Guid UserId { get; set; }
    public required Guid AdminId { get; set; }
}