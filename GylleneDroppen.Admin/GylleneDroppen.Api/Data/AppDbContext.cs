using GylleneDroppen.Api.Data.Configurations;
using GylleneDroppen.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace GylleneDroppen.Api.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; init; }
    public DbSet<Event> Events { get; init; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        
        base.OnModelCreating(modelBuilder);
    }
}