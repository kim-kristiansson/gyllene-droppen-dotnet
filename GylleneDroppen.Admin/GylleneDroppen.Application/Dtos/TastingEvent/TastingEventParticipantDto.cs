namespace GylleneDroppen.Application.Dtos.TastingEvent;

public class TastingEventParticipantDto
{
    public Guid Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string UserEmail { get; set; } = string.Empty;
    public DateTime RegisteredDate { get; set; }
    public bool Attended { get; set; }
    public string? Notes { get; set; }
}