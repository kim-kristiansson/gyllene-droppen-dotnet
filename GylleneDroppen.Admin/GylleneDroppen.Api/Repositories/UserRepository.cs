using GylleneDroppen.Api.Data;
using GylleneDroppen.Api.Enums;
using GylleneDroppen.Api.Models;
using GylleneDroppen.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GylleneDroppen.Api.Repositories;

public class UserRepository(AppDbContext context):Repository<User>(context), IUserRepository
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