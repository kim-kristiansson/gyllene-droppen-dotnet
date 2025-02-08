using GylleneDroppen.Api.Models;

namespace GylleneDroppen.Api.Dtos;

public class CreateEventRequest
{
    public required DateTime StartTime { get; set; }
    public required DateTime EndTime { get; set; }
    public required Location Location { get; set; }
}