using GylleneDroppen.Domain.Entities;

namespace GylleneDroppen.Application.Interfaces.Shared.Repositories;

public interface ITastingRepository : IRepository<Tasting>
{
    Task<List<Tasting>> GetUpcomingTastingsAsync();
    Task<Tasting?> GetRegisterTastingDataAsync(Guid eventId);
}