using GylleneDroppen.Application.Common.Results;
using GylleneDroppen.Application.Dtos.Common;
using GylleneDroppen.Application.Dtos.Public.Tasting;
using GylleneDroppen.Application.Dtos.Tasting;

namespace GylleneDroppen.Application.Interfaces.Public.Services;

public interface ITastingService
{
    Task<Result<List<TastingResponse>>> GetUpcomingTastingsAsync();
    Task<Result<TastingResponse>> GetTastingByIdAsync(Guid tastingId);
    Task<Result<MessageResponse>> RegisterForTastingAsync(Guid userId, RegisterForTastingRequest request);
}