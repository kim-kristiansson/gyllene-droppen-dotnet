using GylleneDroppen.Core.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GylleneDroppen.Infrastructure.Persistence;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext(options)
{
    public DbSet<ApplicationUser> Users { get; set; }
    public DbSet<Whisky> Whiskies { get; set; }
    public DbSet<TastingHistory> TastingHistories { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Whisky configuration
        builder.Entity<Whisky>(entity =>
        {
            entity.HasKey(w => w.Id);

            entity.Property(w => w.Name)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(w => w.Distillery)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(w => w.Abv)
                .HasPrecision(5, 2);

            entity.Property(w => w.Price)
                .HasPrecision(10, 2);

            entity.Property(w => w.Region)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(w => w.Type)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(w => w.Country)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(w => w.Color)
                .HasMaxLength(500);

            entity.Property(w => w.Nose)
                .HasMaxLength(1000);

            entity.Property(w => w.Palate)
                .HasMaxLength(1000);

            entity.Property(w => w.Finish)
                .HasMaxLength(1000);

            entity.Property(w => w.ImagePath)
                .HasMaxLength(500);

            entity.Property(w => w.CreatedByUserId)
                .IsRequired();

            // Relationships
            entity.HasOne(w => w.CreatedByUser)
                .WithMany()
                .HasForeignKey(w => w.CreatedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(w => w.UpdatedByUser)
                .WithMany()
                .HasForeignKey(w => w.UpdatedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Indexes
            entity.HasIndex(w => new { w.Name, w.Distillery })
                .IsUnique()
                .HasDatabaseName("IX_Whisky_Name_Distillery");

            entity.HasIndex(w => w.Country)
                .HasDatabaseName("IX_Whisky_Country");

            entity.HasIndex(w => w.Region)
                .HasDatabaseName("IX_Whisky_Region");

            entity.HasIndex(w => w.Type)
                .HasDatabaseName("IX_Whisky_Type");

            entity.HasIndex(w => w.CreatedDate)
                .HasDatabaseName("IX_Whisky_CreatedDate");
        });

        // TastingHistory configuration
        builder.Entity<TastingHistory>(entity =>
        {
            entity.HasKey(th => th.Id);

            entity.Property(th => th.EventTitle)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(th => th.Notes)
                .HasMaxLength(1000);

            entity.Property(th => th.OrganizedByUserId)
                .IsRequired();

            // Relationships
            entity.HasOne(th => th.Whisky)
                .WithMany(w => w.TastingHistories)
                .HasForeignKey(th => th.WhiskyId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(th => th.OrganizedByUser)
                .WithMany()
                .HasForeignKey(th => th.OrganizedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Indexes
            entity.HasIndex(th => th.WhiskyId)
                .HasDatabaseName("IX_TastingHistory_WhiskyId");

            entity.HasIndex(th => th.TastingDate)
                .HasDatabaseName("IX_TastingHistory_TastingDate");

            entity.HasIndex(th => th.OrganizedByUserId)
                .HasDatabaseName("IX_TastingHistory_OrganizedByUserId");
        });
    }
}