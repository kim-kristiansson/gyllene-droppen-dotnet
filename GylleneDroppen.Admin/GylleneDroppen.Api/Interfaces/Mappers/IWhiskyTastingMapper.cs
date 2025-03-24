using GylleneDroppen.Api.Dtos.WhiskyTasting;
using GylleneDroppen.Api.Queries.WhiskyTasting;

namespace GylleneDroppen.Api.Interfaces.Mappers;

public interface IWhiskyTastingMapper
{
    WhiskyTastingResponse ToWhiskyTastingResponse(UpcomingWhiskyTastingQuery query);
    List<WhiskyTastingResponse> ToWhiskyTastingResponse(List<UpcomingWhiskyTastingQuery> queries);
}