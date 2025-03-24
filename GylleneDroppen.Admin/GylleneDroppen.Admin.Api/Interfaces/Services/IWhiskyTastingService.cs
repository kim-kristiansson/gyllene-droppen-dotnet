using GylleneDroppen.Admin.Api.Dtos.WhiskyTasting;
using GylleneDroppen.Shared.Dtos.Generic;
using GylleneDroppen.Shared.Utils;

namespace GylleneDroppen.Admin.Api.Interfaces.Services;

public interface IWhiskyTastingService
{
    Task<ServiceResponse<MessageResponse>> CreateWhiskyTastingAsync(CreateWhiskyTastingRequest request);
    Task<ServiceResponse<List<WhiskyTastingResponse>>> GetUpcomingWhiskyTastingsAsync();
    Task<ServiceResponse<List<WhiskyTastingResponse>>> GetAllWhiskyTastingsAsync();
    Task<ServiceResponse<MessageResponse>> UpdateWhiskyTastingAsync(UpdateWhiskyTastingRequest request);
}