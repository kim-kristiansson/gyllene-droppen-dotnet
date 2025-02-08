using Microsoft.EntityFrameworkCore;

namespace GylleneDroppen.Api.Data;

public class ClubDbContext(DbContextOptions<ClubDbContext> options) : DbContext(options);