using GylleneDroppen.Core.Entities;

namespace GylleneDroppen.Application.Interfaces.Repositories;

public interface ITastingEventWhiskyRepository : IRepository<TastingEventWhisky>
{
    Task<TastingEventWhisky?> GetByEventAndWhiskyAsync(Guid eventId, Guid whiskyId);
    Task<List<TastingEventWhisky>> GetByEventIdAsync(Guid eventId);
    Task<bool> ExistsAsync(Guid eventId, Guid whiskyId);
    Task UpdateOrdersAsync(Guid eventId, List<(Guid WhiskyId, int Order)> orders);
}