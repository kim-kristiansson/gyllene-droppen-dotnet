namespace GylleneDroppen.Application.Dtos.WhiskyMetadata;

public class RegionDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public Guid CountryId { get; set; }
    public string CountryName { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public string CreatedByUserName { get; set; } = string.Empty;
    public string? UpdatedByUserName { get; set; }
}

public class CreateRegionRequestDto
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public Guid CountryId { get; set; }
}

public class UpdateRegionRequestDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public Guid CountryId { get; set; }
}