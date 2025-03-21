using GylleneDroppen.Api.Dtos;
using GylleneDroppen.Api.Dtos.Event;
using GylleneDroppen.Api.Models;
using GylleneDroppen.Api.Queries.Event;

namespace GylleneDroppen.Api.Repositories.Interfaces;

public interface IEventRepository : IRepository<Event>
{
    Task<List<EventUserResponse>> GetUpcomingEventsAsync();
    Task<RegisterEventQuery?> GetRegisterEventDataAsync(Guid eventId);
}