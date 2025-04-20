using GylleneDroppen.Application.Queries;
using GylleneDroppen.Domain.Entities;

namespace GylleneDroppen.Application.Interfaces.Shared.Repositories;

public interface ITastingRepository : IRepository<Tasting>
{
    Task<List<UpcomingTastingsQuery>> GetUpcomingTastingsAsync();
    Task<RegisterTastingQuery?> GetRegisterTastingDataAsync(Guid eventId);
}