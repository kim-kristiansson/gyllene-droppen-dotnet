using GylleneDroppen.Api.Dtos.WhiskyTasting;
using GylleneDroppen.Shared.Dtos.Generic;
using GylleneDroppen.Shared.Utils;

namespace GylleneDroppen.Api.Interfaces.Services;

public interface IWhiskyTastingService
{
    Task<ServiceResponse<List<WhiskyTastingResponse>>> GetUpcomingEventsAsync();

    Task<ServiceResponse<MessageResponse>> RegisterForWhiskyTastingAsync(RegisterForWhiskyTastingRequest request,
        Guid userId);
}