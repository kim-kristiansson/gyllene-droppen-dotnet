using GylleneDroppen.Application.Interfaces.Repositories;
using GylleneDroppen.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace GylleneDroppen.Infrastructure.Persistence.Repositories;

public class DepartmentRepository(ApplicationDbContext context)
    : Repository<Department>(context), IDepartmentRepository
{
    public async Task<List<Department>> GetAllDepartmentsAsync()
    {
        return await DbSet
            .Include(d => d.CreatedByUser)
            .Include(d => d.UpdatedByUser)
            .OrderBy(d => d.Name)
            .ToListAsync();
    }

    public async Task<List<Department>> GetActiveDepartmentsAsync()
    {
        return await DbSet
            .Where(d => d.IsActive)
            .OrderBy(d => d.Name)
            .ToListAsync();
    }

}