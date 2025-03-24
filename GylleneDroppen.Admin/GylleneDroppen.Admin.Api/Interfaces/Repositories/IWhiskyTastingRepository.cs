using GylleneDroppen.Admin.Api.Queries.WhiskyTasting;
using GylleneDroppen.Core.Entities;
using GylleneDroppen.Core.Interfaces.Repositories;

namespace GylleneDroppen.Admin.Api.Interfaces.Repositories;

public interface IWhiskyTastingRepository : IRepository<WhiskyTasting>
{
    Task<List<UpcomingWhiskyTastingQuery>> GetUpcomingWhiskyTastingAsync();
    Task<RegisterForWhiskyTastingQuery?> GetRegisterWhiskyTastingDataAsync(Guid eventId);
}