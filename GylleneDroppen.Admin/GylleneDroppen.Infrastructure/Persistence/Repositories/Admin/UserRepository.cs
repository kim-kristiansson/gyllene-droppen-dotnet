using GylleneDroppen.Application.Interfaces.Shared.Repositories;
using GylleneDroppen.Domain.Entities;
using GylleneDroppen.Domain.Enums;
using GylleneDroppen.Infrastructure.Persistence.Data;
using GylleneDroppen.Infrastructure.Persistence.Repositories.Shared;
using Microsoft.EntityFrameworkCore;

namespace GylleneDroppen.Infrastructure.Persistence.Repositories.Admin;

public class UserRepository(AppDbContext context) : Repository<User>(context), IUserRepository
{
    public async Task<User?> GetByEmailAsync(string email)
    {
        return await DbSet.FirstOrDefaultAsync(x => x.Email == email);
    }

    public async Task<List<User>> GetUsersByRoleAsync(RoleType role)
    {
        return await DbSet.Where(u => u.Role == role).ToListAsync();
    }
}