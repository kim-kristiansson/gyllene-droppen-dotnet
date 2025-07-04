using GylleneDroppen.Application.Interfaces.Repositories;
using GylleneDroppen.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace GylleneDroppen.Infrastructure.Persistence.Repositories;

public class AddressRepository(ApplicationDbContext context)
    : Repository<Address>(context), IAddressRepository
{
    public async Task<List<Address>> GetAllAddressesAsync()
    {
        return await DbSet
            .Include(a => a.CreatedByUser)
            .Include(a => a.UpdatedByUser)
            .OrderBy(a => a.Name)
            .ToListAsync();
    }

    public async Task<List<Address>> GetActiveAddressesAsync()
    {
        return await DbSet
            .Where(a => a.IsActive)
            .OrderBy(a => a.Name)
            .ToListAsync();
    }

    public async Task<Address?> GetAddressWithDetailsAsync(Guid id)
    {
        return await DbSet
            .Include(a => a.CreatedByUser)
            .Include(a => a.UpdatedByUser)
            .FirstOrDefaultAsync(a => a.Id == id);
    }

    public new async Task<Address?> GetByIdAsync(Guid id)
    {
        return await GetAddressWithDetailsAsync(id);
    }
}