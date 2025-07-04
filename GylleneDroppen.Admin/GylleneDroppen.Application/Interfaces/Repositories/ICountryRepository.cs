using GylleneDroppen.Core.Entities;

namespace GylleneDroppen.Application.Interfaces.Repositories;

public interface ICountryRepository : IRepository<Country>
{
    Task<bool> ExistsWithNameAsync(string name, Guid? excludeId = null);
    Task<Country?> GetWithRegionsAsync(Guid id);
}