namespace GylleneDroppen.Application.Dtos.WhiskyMetadata;

public class WhiskyTypeDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public string CreatedByUserName { get; set; } = string.Empty;
    public string? UpdatedByUserName { get; set; }
    
    // Origin information
    public Guid? OriginCountryId { get; set; }
    public string? OriginCountryName { get; set; }
    public Guid? OriginRegionId { get; set; }
    public string? OriginRegionName { get; set; }
}

public class CreateWhiskyTypeRequestDto
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public Guid? OriginCountryId { get; set; }
    public Guid? OriginRegionId { get; set; }
}

public class UpdateWhiskyTypeRequestDto
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public Guid? OriginCountryId { get; set; }
    public Guid? OriginRegionId { get; set; }
}