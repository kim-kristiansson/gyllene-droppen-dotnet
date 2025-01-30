using GylleneDroppen.Admin.Api.Data.Configurations;
using GylleneDroppen.Admin.Api.Extensions;
using GylleneDroppen.Admin.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace GylleneDroppen.Admin.Api.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; init; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        base.OnModelCreating(modelBuilder);
    }
}