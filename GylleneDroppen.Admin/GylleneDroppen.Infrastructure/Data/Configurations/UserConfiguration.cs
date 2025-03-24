using GylleneDroppen.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GylleneDroppen.Infrastructure.Data.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.Property(u => u.Email)
            .HasMaxLength(320)
            .IsRequired();

        builder.Property(u => u.PasswordHash)
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(u => u.PasswordSalt)
            .HasMaxLength(32)
            .IsRequired();
    }
}