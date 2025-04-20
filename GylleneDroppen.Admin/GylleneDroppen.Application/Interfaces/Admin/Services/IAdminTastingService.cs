using GylleneDroppen.Application.Common.Results;
using GylleneDroppen.Application.Dtos.Common;
using GylleneDroppen.Application.Dtos.Tasting;

namespace GylleneDroppen.Application.Interfaces.Admin.Services;

public interface IAdminTastingService
{
    Task<Result<List<TastingAdminResponse>>> GetAllUpcomingTastingsAsync();
    Task<Result<List<TastingAdminResponse>>> GetAllTastingsAsync();
    Task<Result<MessageResponse>> CreateTastingAsync(CreateTastingRequest request);
    Task<Result<MessageResponse>> UpdateTastingAsync(UpdateTastingRequest request);
}