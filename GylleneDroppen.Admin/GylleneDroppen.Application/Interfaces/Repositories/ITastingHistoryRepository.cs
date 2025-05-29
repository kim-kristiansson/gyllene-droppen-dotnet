using GylleneDroppen.Core.Entities;

namespace GylleneDroppen.Application.Interfaces.Repositories;

public interface ITastingHistoryRepository : IRepository<TastingHistory>
{
    Task<List<TastingHistory>> GetTastingHistoryByWhiskyIdAsync(Guid whiskyId);
    Task<List<TastingHistory>> GetRecentTastingHistoryAsync(int count = 10);
    Task<List<TastingHistory>> GetTastingHistoryByUserAsync(string userId);
}