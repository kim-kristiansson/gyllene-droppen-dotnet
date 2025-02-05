using GylleneDroppen.Admin.Api.Data;
using GylleneDroppen.Admin.Api.Models;
using GylleneDroppen.Admin.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GylleneDroppen.Admin.Api.Repositories;

public class UserRepository(AppDbContext context):Repository<User>(context), IUserRepository
{
    public async Task<User?> GetByEmailAsync(string email)
    {
        return await DbSet.FirstOrDefaultAsync(x => x.Email == email);
    }
}