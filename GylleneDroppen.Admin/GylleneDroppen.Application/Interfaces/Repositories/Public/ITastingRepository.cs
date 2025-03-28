namespace GylleneDroppen.Application.Interfaces.Repositories.Public;

public interface ITastingRepository : IRepository<Tasting>
{
    Task<List<EventUserResponse>> GetUpcomingEventsAsync();
    Task<RegisterEventQuery?> GetRegisterEventDataAsync(Guid eventId);
}