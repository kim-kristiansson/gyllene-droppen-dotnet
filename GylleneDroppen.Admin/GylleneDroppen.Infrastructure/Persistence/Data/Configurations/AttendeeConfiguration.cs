using GylleneDroppen.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GylleneDroppen.Infrastructure.Persistence.Data.Configurations;

public class AttendeeConfiguration : IEntityTypeConfiguration<Attendee>
{
    public void Configure(EntityTypeBuilder<Attendee> builder)
    {
        builder.HasKey(p => new { p.UserId, p.TastingId });

        builder.HasOne(p => p.User)
            .WithMany(u => u.Attendees)
            .HasForeignKey(p => p.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(p => p.Tasting)
            .WithMany(e => e.Attendees)
            .HasForeignKey(p => p.TastingId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}