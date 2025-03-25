using GylleneDroppen.Admin.Api.Dtos.WhiskyTasting;
using GylleneDroppen.Admin.Api.Queries.WhiskyTasting;
using GylleneDroppen.Core.Dtos.WhiskyTasting.Shared;
using GylleneDroppen.Core.Entities;

namespace GylleneDroppen.Admin.Api.Interfaces.Mappers;

public interface IWhiskyTastingMapper
{
    WhiskyTasting ToWhiskyTasting(CreateWhiskyTastingRequest request);
    WhiskyTastingAdminResponse ToWhiskyTastingResponse(WhiskyTasting whiskyTasting);
    List<WhiskyTastingBaseResponse> ToWhiskyTastingResponse(List<WhiskyTasting> whiskyTastings);
    WhiskyTastingBaseResponse ToWhiskyTastingResponse(UpcomingWhiskyTastingQuery query);
    List<WhiskyTastingAdminResponse> ToWhiskyTastingResponse(List<UpcomingWhiskyTastingQuery> queries);
}