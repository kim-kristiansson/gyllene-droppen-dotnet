using GylleneDroppen.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GylleneDroppen.Infrastructure.Persistence.Data.Configurations;

public class MembershipConfiguration : IEntityTypeConfiguration<Membership>
{
    public void Configure(EntityTypeBuilder<Membership> builder)
    {
        // Primary key
        builder.HasKey(m => m.Id);

        // 1-to-1 relationship with User
        builder.HasOne(m => m.User)
            .WithOne(u => u.Membership)
            .HasForeignKey<Membership>(m => m.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // Required fields
        builder.Property(m => m.StartDate)
            .IsRequired();

        builder.Property(m => m.Status)
            .IsRequired();

        builder.Property(m => m.Type)
            .IsRequired();

        builder.Property(m => m.CreatedAt)
            .IsRequired();

        builder.Property(m => m.Notes)
            .HasMaxLength(500);

        builder.Property(m => m.PaymentReference)
            .HasMaxLength(100);
    }
}