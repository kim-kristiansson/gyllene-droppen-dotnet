using GylleneDroppen.Core.Entities;

namespace GylleneDroppen.Application.Interfaces.Repositories;

public interface ITastingEventParticipantRepository : IRepository<TastingEventParticipant>
{
    Task<TastingEventParticipant?> GetByEventAndUserAsync(Guid eventId, string userId);
    Task<List<TastingEventParticipant>> GetByEventIdAsync(Guid eventId);
    Task<List<TastingEventParticipant>> GetByUserIdAsync(string userId);
    Task<bool> IsUserRegisteredAsync(Guid eventId, string userId);
    Task<int> GetParticipantCountAsync(Guid eventId);
}