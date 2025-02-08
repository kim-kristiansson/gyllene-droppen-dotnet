using Microsoft.EntityFrameworkCore;

namespace GylleneDroppen.Admin.Api.Data;

public class ClubDbContext(DbContextOptions<ClubDbContext> options) : DbContext(options);