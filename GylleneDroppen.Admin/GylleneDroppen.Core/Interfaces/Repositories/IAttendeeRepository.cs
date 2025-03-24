using GylleneDroppen.Core.Entities;

namespace GylleneDroppen.Core.Interfaces.Repositories;

public interface IAttendeeRepository : IRepository<Attendee>
{
    Task<bool> IsUserRegisteredAsync(Guid userId, Guid eventId);
}