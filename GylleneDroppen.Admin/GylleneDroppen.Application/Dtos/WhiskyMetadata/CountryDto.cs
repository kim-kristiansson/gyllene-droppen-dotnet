namespace GylleneDroppen.Application.Dtos.WhiskyMetadata;

public class CountryDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public string CreatedByUserName { get; set; } = string.Empty;
    public string? UpdatedByUserName { get; set; }
    public int RegionCount { get; set; }
}

public class CreateCountryRequestDto
{
    public string Name { get; set; } = string.Empty;
}

public class UpdateCountryRequestDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
}