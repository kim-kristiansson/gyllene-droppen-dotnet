using GylleneDroppen.Api.Models;

namespace GylleneDroppen.Api.Repositories.Interfaces;

public interface IParticipantRepository : IRepository<Participant>
{
    Task<bool> IsUserRegisteredAsync(Guid userId, Guid eventId);
}