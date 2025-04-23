using GylleneDroppen.Application.Common.Results;
using GylleneDroppen.Application.Dtos.Common;
using GylleneDroppen.Application.Dtos.Tasting;

namespace GylleneDroppen.Application.Interfaces.Public.Services;

public interface ITastingService
{
    Task<Result<List<TastingUserResponse>>> GetUpcomingTastingsAsync();
    Task<Result<TastingUserResponse>> GetTastingByIdAsync(Guid tastingId);
    Task<Result<MessageResponse>> RegisterForTastingAsync(Guid userId, RegisterForTastingRequest request);
}