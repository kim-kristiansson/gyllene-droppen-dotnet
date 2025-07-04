using GylleneDroppen.Core.Entities;

namespace GylleneDroppen.Application.Interfaces.Repositories;

public interface IRegionRepository : IRepository<Region>
{
    Task<List<Region>> GetRegionsByCountryAsync(Guid countryId);
    Task<bool> ExistsWithNameInCountryAsync(string name, Guid countryId, Guid? excludeId = null);
}