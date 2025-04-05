using GylleneDroppen.Application.Common.Results;
using GylleneDroppen.Application.Dtos.Common;
using GylleneDroppen.Application.Dtos.Tasting;

namespace GylleneDroppen.Application.Interfaces.Services.Admin;

public interface ITastingService
{
    Task<Result<MessageResponse>> CreateTastingAsync(CreateTastingRequest request);
    Task<Result<List<TastingUserResponse>>> GetUpcomingTastingsAsync();
    Task<Result<List<TastingAdminResponse>>> GetAllTastingsAsync();
    Task<Result<MessageResponse>> RegisterForTastingAsync(RegisterForTastingRequest request, Guid userId);
    Task<Result<MessageResponse>> UpdateTastingAsync(UpdateTastingRequest request);
}