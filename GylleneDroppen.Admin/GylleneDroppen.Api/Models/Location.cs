namespace GylleneDroppen.Api.Models;

public class Location
{
    public required Guid Id { get; set; }
    public required string Address { get; set; }
    public required string City { get; set; }
}