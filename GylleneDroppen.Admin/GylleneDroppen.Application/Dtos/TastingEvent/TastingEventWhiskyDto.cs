namespace GylleneDroppen.Application.Dtos.TastingEvent;

public class TastingEventWhiskyDto
{
    public Guid Id { get; set; }
    public Guid WhiskyId { get; set; }
    public string WhiskyName { get; set; } = string.Empty;
    public string Distillery { get; set; } = string.Empty;
    public int Order { get; set; }
    public string? Notes { get; set; }
}