using GylleneDroppen.Application.Interfaces.Repositories;
using GylleneDroppen.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace GylleneDroppen.Infrastructure.Persistence.Repositories;

public class UserRepository(ApplicationDbContext context) : Repository<ApplicationUser>(context), IUserRepository
{
    public async Task<ApplicationUser?> GetByEmailAsync(string email)
    {
        return await DbSet.FirstOrDefaultAsync(x => x.Email == email);
    }

    public async Task<bool> ExistsByEmailAsync(string email)
    {
        return await DbSet.AnyAsync(x => x.Email == email);
    }
}