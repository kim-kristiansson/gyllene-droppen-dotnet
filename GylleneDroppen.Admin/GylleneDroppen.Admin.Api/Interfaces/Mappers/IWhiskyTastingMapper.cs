using GylleneDroppen.Admin.Api.Dtos.WhiskyTasting;
using GylleneDroppen.Admin.Api.Queries.WhiskyTasting;
using GylleneDroppen.Core.Entities;

namespace GylleneDroppen.Admin.Api.Interfaces.Mappers;

public interface IWhiskyTastingMapper
{
    WhiskyTasting ToWhiskyTasting(CreateWhiskyTastingRequest request);
    WhiskyTastingResponse ToWhiskyTastingResponse(WhiskyTasting whiskyTasting);
    List<WhiskyTastingResponse> ToWhiskyTastingResponse(List<WhiskyTasting> whiskyTastings);
    WhiskyTastingResponse ToWhiskyTastingResponse(UpcomingWhiskyTastingQuery query);
    List<WhiskyTastingResponse> ToWhiskyTastingResponse(List<UpcomingWhiskyTastingQuery> queries);
}