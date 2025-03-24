using GylleneDroppen.Admin.Api.Dtos.WhiskyTasting;
using GylleneDroppen.Admin.Api.Interfaces.Mappers;
using GylleneDroppen.Admin.Api.Queries.WhiskyTasting;
using GylleneDroppen.Core.Entities;
using GylleneDroppen.Shared.Dtos.Generic;

namespace GylleneDroppen.Api.Mappers;

public class WhiskyTastingMapper : IWhiskyTastingMapper
{
    public WhiskyTasting ToWhiskyTasting(CreateWhiskyTastingRequest request)
    {
        throw new NotImplementedException();
    }

    public WhiskyTastingResponse ToWhiskyTastingResponse(WhiskyTasting whiskyTasting)
    {
        throw new NotImplementedException();
    }

    public List<WhiskyTastingResponse> ToWhiskyTastingResponse(List<WhiskyTasting> whiskyTastings)
    {
        throw new NotImplementedException();
    }

    public WhiskyTastingResponse ToWhiskyTastingResponse(UpcomingWhiskyTastingQuery query)
    {
        return new WhiskyTastingResponse
        {
            Id = query.Id,
            Title = query.Title,
            Description = query.Description,
            StartTime = query.StartTime,
            EndTime = query.EndTime,
            Location = query.Location,
            Capacity = query.Capacity,
            Price = query.Price,
            Deadline = query.Deadline,
            Organizer = new OrganizerResponse
            {
                Id = query.Organizer.Id,
                Name = query.Organizer.Name
            }
        };
    }

    public List<WhiskyTastingResponse> ToWhiskyTastingResponse(List<UpcomingWhiskyTastingQuery> queries)
    {
        return queries.Select(ToWhiskyTastingResponse).ToList();
    }
}