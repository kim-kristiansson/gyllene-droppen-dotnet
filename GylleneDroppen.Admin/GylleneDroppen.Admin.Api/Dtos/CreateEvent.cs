using GylleneDroppen.Admin.Api.Models;

namespace GylleneDroppen.Admin.Api.Dtos;

public class CreateEventRequest
{
    public required DateTime StartTime { get; set; }
    public required DateTime EndTime { get; set; }
    public required Location Location { get; set; }
}