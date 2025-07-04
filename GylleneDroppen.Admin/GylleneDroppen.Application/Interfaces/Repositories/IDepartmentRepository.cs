using GylleneDroppen.Core.Entities;

namespace GylleneDroppen.Application.Interfaces.Repositories;

public interface IDepartmentRepository : IRepository<Department>
{
    Task<List<Department>> GetAllDepartmentsAsync();
    Task<List<Department>> GetActiveDepartmentsAsync();
}