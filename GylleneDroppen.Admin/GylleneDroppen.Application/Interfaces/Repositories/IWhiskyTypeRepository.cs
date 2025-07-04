using GylleneDroppen.Core.Entities;

namespace GylleneDroppen.Application.Interfaces.Repositories;

public interface IWhiskyTypeRepository : IRepository<WhiskyType>
{
    Task<bool> ExistsWithNameAsync(string name, Guid? excludeId = null);
    Task<List<WhiskyType>> GetAllWithOriginsAsync();
}