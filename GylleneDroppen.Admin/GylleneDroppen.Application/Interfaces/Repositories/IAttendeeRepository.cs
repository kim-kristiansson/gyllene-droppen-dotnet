using GylleneDroppen.Application.Interfaces.Shared.Repositories;
using GylleneDroppen.Domain.Entities;

namespace GylleneDroppen.Application.Interfaces.Repositories;

public interface IAttendeeRepository : IRepository<Attendee>
{
    Task<bool> IsUserRegisteredAsync(Guid userId, Guid eventId);
    Task<int> CountUserAttendancesAsync(Guid userId);
}