namespace GylleneDroppen.Application.Dtos.Whisky;

public class WhiskySearchResultDto
{
    public List<WhiskyResponseDto> Whiskies { get; set; } = [];
    public int TotalCount { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
    public bool HasPreviousPage => Page > 1;
    public bool HasNextPage => Page < TotalPages;
}