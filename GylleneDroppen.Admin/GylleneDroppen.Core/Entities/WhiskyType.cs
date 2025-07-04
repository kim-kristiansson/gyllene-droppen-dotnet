using System.ComponentModel.DataAnnotations;

namespace GylleneDroppen.Core.Entities;

public class WhiskyType
{
    public Guid Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? Description { get; set; }

    // Origin tracking - where this whisky type originated
    public Guid? OriginCountryId { get; set; }
    public Country? OriginCountry { get; set; }

    public Guid? OriginRegionId { get; set; }
    public Region? OriginRegion { get; set; }

    public DateTime CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }

    [Required]
    public string CreatedByUserId { get; set; } = string.Empty;
    public ApplicationUser CreatedByUser { get; set; } = null!;

    public string? UpdatedByUserId { get; set; }
    public ApplicationUser? UpdatedByUser { get; set; }

    // Navigation properties
    public ICollection<Whisky> Whiskies { get; set; } = new List<Whisky>();
}