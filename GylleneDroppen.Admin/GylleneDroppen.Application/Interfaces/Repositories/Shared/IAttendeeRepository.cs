namespace GylleneDroppen.Application.Interfaces.Repositories.Shared;

public interface IAttendeeRepository : IRepository<Attendee>
{
    Task<bool> IsUserRegisteredAsync(Guid userId, Guid eventId);
}