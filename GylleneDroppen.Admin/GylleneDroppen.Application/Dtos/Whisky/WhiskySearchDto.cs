namespace GylleneDroppen.Application.Dtos.Whisky;

public class WhiskySearchDto
{
    public string? SearchTerm { get; set; }
    public string? Country { get; set; }
    public string? Region { get; set; }
    public string? Type { get; set; }
    public int? MinAge { get; set; }
    public int? MaxAge { get; set; }
    public decimal? MinAbv { get; set; }
    public decimal? MaxAbv { get; set; }
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
    public string? SortBy { get; set; } = "Name"; // Name, Age, Abv, Price, CreatedDate
    public bool SortDescending { get; set; } = false;
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}