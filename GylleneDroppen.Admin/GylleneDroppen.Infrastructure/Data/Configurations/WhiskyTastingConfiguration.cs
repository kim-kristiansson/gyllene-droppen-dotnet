using GylleneDroppen.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GylleneDroppen.Infrastructure.Data.Configurations;

public class WhiskyTastingConfiguration : IEntityTypeConfiguration<WhiskyTasting>
{
    public void Configure(EntityTypeBuilder<WhiskyTasting> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Title)
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(e => e.Description)
            .HasMaxLength(1000)
            .IsRequired();

        builder.Property(e => e.Location)
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(e => e.Capacity)
            .IsRequired();

        builder.Property(e => e.Price)
            .IsRequired();

        builder.Property(e => e.StartTime)
            .IsRequired();

        builder.Property(e => e.EndTime)
            .IsRequired();

        builder.Property(e => e.Deadline)
            .IsRequired();

        builder.Property(e => e.CreatedAt)
            .IsRequired();

        builder.Property(e => e.CreatedById)
            .IsRequired();

        builder.Property(e => e.OrganizerId)
            .IsRequired();

        builder.HasOne(e => e.Organizer)
            .WithMany()
            .HasForeignKey(e => e.OrganizerId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

        builder.HasOne(e => e.CreatedBy)
            .WithMany()
            .HasForeignKey(e => e.CreatedById)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(e => e.Attendees)
            .WithOne(a => a.Event as WhiskyTasting)
            .HasForeignKey(a => a.EventId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}