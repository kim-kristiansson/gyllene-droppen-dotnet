using GylleneDroppen.Core.Entities;
using GylleneDroppen.Infrastructure.Data.Configurations;
using Microsoft.EntityFrameworkCore;

namespace GylleneDroppen.Infrastructure.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; init; }
    public DbSet<Event> Events { get; init; }
    public DbSet<WhiskyTasting> WhiskyTastings { get; init; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new WhiskyTastingConfiguration());
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new ParticipantConfiguration());

        base.OnModelCreating(modelBuilder);
    }
}