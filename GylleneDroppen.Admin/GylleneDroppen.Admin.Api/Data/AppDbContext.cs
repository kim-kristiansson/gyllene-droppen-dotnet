using GylleneDroppen.Admin.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace GylleneDroppen.Admin.Api.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; init; }
}