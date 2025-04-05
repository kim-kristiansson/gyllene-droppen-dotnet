using GylleneDroppen.Application.Interfaces.Repositories.Shared;
using GylleneDroppen.Application.Queries;
using GylleneDroppen.Domain.Entities;

namespace GylleneDroppen.Application.Interfaces.Repositories.Public;

public interface ITastingRepository : IRepository<Tasting>
{
    Task<List<UpcomingTastingsQuery>> GetUpcomingTastingsAsync();
    Task<RegisterTastingQuery?> GetRegisterTastingDataAsync(Guid eventId);
}