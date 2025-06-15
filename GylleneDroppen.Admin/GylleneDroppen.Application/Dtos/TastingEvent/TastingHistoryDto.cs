namespace GylleneDroppen.Application.Dtos.Tasting;

public class TastingHistoryDto
{
    public Guid Id { get; set; }
    public Guid WhiskyId { get; set; }
    public string WhiskyName { get; set; } = string.Empty;
    public string EventTitle { get; set; } = string.Empty;
    public DateTime TastingDate { get; set; }
    public string? Notes { get; set; }
    public string OrganizedByUserName { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; }
}