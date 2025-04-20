using GylleneDroppen.Domain.Entities;

namespace GylleneDroppen.Application.Interfaces.Shared.Repositories;

public interface IAttendeeRepository : IRepository<Attendee>
{
    Task<bool> IsUserRegisteredAsync(Guid userId, Guid eventId);
}