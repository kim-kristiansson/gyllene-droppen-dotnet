using System.ComponentModel.DataAnnotations;
using GylleneDroppen.Core.Entities;

public class Whisky
{
    public Guid Id { get; set; }

    [Required] [MaxLength(200)] public string Name { get; set; } = string.Empty;

    [Required] [MaxLength(200)] public string Distillery { get; set; } = string.Empty;

    public int? Age { get; set; } // Nullable for NAS (No Age Statement) whiskies

    [Range(0, 100)] public decimal Abv { get; set; } // Alcohol by Volume percentage

    [Required] [MaxLength(100)] public string Region { get; set; } = string.Empty;

    [Required] [MaxLength(100)] public string Type { get; set; } = string.Empty; // Single Malt, Blend, etc.

    [Required] [MaxLength(100)] public string Country { get; set; } = string.Empty;

    // Tasting notes
    [MaxLength(500)] public string? Color { get; set; }

    [MaxLength(1000)] public string? Nose { get; set; }

    [MaxLength(1000)] public string? Palate { get; set; }

    [MaxLength(1000)] public string? Finish { get; set; }

    // Additional details
    [Range(0, double.MaxValue)] public decimal? Price { get; set; }

    [Range(0, 10000)] public int? BottleSize { get; set; } // in ml

    [MaxLength(500)] public string? ImagePath { get; set; }

    // Metadata
    public DateTime CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }

    [Required] public string CreatedByUserId { get; set; } = string.Empty;
    public ApplicationUser CreatedByUser { get; set; } = null!;

    public string? UpdatedByUserId { get; set; }
    public ApplicationUser? UpdatedByUser { get; set; }

    // Navigation properties
    public ICollection<TastingHistory> TastingHistories { get; set; } = new List<TastingHistory>();
}