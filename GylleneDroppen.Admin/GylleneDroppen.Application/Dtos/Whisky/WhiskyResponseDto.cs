namespace GylleneDroppen.Application.Dtos.Whisky;

public class WhiskyResponseDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Distillery { get; set; } = string.Empty;
    public int? Age { get; set; }
    public decimal Abv { get; set; }
    public string Region { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public string? Color { get; set; }
    public string? Nose { get; set; }
    public string? Palate { get; set; }
    public string? Finish { get; set; }
    public decimal? Price { get; set; }
    public int? BottleSize { get; set; }
    public string? ImagePath { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public string CreatedByUserName { get; set; } = string.Empty;
    public string? UpdatedByUserName { get; set; }
    public int TastingCount { get; set; }
}