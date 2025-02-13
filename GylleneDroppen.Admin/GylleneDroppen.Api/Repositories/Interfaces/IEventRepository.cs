using GylleneDroppen.Api.Models;

namespace GylleneDroppen.Api.Repositories.Interfaces;

public interface IEventRepository : IRepository<Event>
{
    Task<List<Event>> GetUpcomingEventsAsync();
}