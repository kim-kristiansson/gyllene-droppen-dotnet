using GylleneDroppen.Api.Queries.WhiskyTasting;

namespace GylleneDroppen.Api.Interfaces.Repositories;

public interface IWhiskyTastingRepository
{
    Task<List<UpcomingWhiskyTastingQuery>> GetUpcomingWhiskyTastingAsync();
    Task<RegisterForWhiskyTastingQuery?> GetRegisterWhiskyTastingDataAsync(Guid eventId);
}