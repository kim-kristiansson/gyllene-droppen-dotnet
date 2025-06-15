namespace GylleneDroppen.Application.Interfaces.Repositories;

public interface ITastingEventRepository : IRepository<TastingEvent>
{
    Task<TastingEvent?> GetTastingEventWithDetailsAsync(Guid id);
    Task<List<TastingEvent>> GetUpcomingTastingEventsAsync(int count = 10);
    Task<List<TastingEvent>> GetPastTastingEventsAsync(int count = 10);
    Task<List<TastingEvent>> GetTastingEventsByOrganizerAsync(string userId);
    Task<List<TastingEvent>> GetPublicTastingEventsAsync();
}