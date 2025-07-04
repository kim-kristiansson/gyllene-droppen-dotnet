using System.ComponentModel.DataAnnotations;

namespace GylleneDroppen.Core.Entities;

public class Country
{
    public Guid Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    public DateTime CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }

    [Required]
    public string CreatedByUserId { get; set; } = string.Empty;
    public ApplicationUser CreatedByUser { get; set; } = null!;

    public string? UpdatedByUserId { get; set; }
    public ApplicationUser? UpdatedByUser { get; set; }

    // Navigation properties
    public ICollection<Region> Regions { get; set; } = new List<Region>();
    // Whiskies are accessed through: Country -> Regions -> Whiskies
    // No direct relationship needed
}