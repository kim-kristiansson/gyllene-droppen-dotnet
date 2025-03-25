using GylleneDroppen.Api.Dtos.WhiskyTasting;
using GylleneDroppen.Core.Common;
using GylleneDroppen.Core.Dtos.Generic;

namespace GylleneDroppen.Api.Interfaces.Services;

public interface IWhiskyTastingService
{
    Task<Result<List<WhiskyTastingResponse>>> GetUpcomingEventsAsync();

    Task<Result<MessageResponse>> RegisterForWhiskyTastingAsync(RegisterForWhiskyTastingRequest request,
        Guid userId);
}