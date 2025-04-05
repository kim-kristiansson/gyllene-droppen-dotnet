using GylleneDroppen.Domain.Entities;
using GylleneDroppen.Infrastructure.Persistence.Data.Configurations;
using Microsoft.EntityFrameworkCore;

namespace GylleneDroppen.Infrastructure.Persistence.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; init; }
    public DbSet<Tasting> Tastings { get; init; }
    public DbSet<Attendee> Attendees { get; init; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new TastingConfiguration());
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new AttendeeConfiguration());

        base.OnModelCreating(modelBuilder);
    }
}