namespace GylleneDroppen.Application.Dtos.TastingEvent;

public class TastingEventDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime EventDate { get; set; }
    public string? Location { get; set; }
    public int? MaxParticipants { get; set; }
    public bool IsPublic { get; set; }
    public string OrganizedByUserName { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; }
    public int WhiskyCount { get; set; }
    public int ParticipantCount { get; set; }
    public List<TastingEventWhiskyDto> Whiskies { get; set; } = new();
    public List<TastingEventParticipantDto> Participants { get; set; } = new();
}