using GylleneDroppen.Admin.Api.Dtos.WhiskyTasting;
using GylleneDroppen.Core.Common;
using GylleneDroppen.Core.Dtos.Generic;

namespace GylleneDroppen.Admin.Api.Interfaces.Services;

public interface IWhiskyTastingService
{
    Task<Result<MessageResponse>> CreateWhiskyTastingAsync(CreateWhiskyTastingRequest request);
    Task<Result<List<WhiskyTastingAdminResponse>>> GetUpcomingWhiskyTastingsAsync();
    Task<Result<List<WhiskyTastingAdminResponse>>> GetAllWhiskyTastingsAsync();
    Task<Result<MessageResponse>> UpdateWhiskyTastingAsync(UpdateWhiskyTastingRequest request);
}