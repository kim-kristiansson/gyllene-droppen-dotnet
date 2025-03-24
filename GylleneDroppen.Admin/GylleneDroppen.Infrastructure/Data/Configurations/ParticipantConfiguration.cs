using GylleneDroppen.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GylleneDroppen.Infrastructure.Data.Configurations;

public class ParticipantConfiguration : IEntityTypeConfiguration<Attendee>
{
    public void Configure(EntityTypeBuilder<Attendee> builder)
    {
        builder.HasKey(p => new { p.UserId, p.EventId });

        builder.HasOne(p => p.User)
            .WithMany(u => u.Participations)
            .HasForeignKey(p => p.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(p => p.Event)
            .WithMany(e => e.Attendees)
            .HasForeignKey(p => p.EventId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}