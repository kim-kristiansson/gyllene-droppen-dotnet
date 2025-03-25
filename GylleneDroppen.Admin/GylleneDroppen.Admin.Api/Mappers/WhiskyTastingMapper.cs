using GylleneDroppen.Admin.Api.Dtos.WhiskyTasting;
using GylleneDroppen.Admin.Api.Interfaces.Mappers;
using GylleneDroppen.Admin.Api.Queries.WhiskyTasting;
using GylleneDroppen.Core.Dtos.Generic;
using GylleneDroppen.Core.Entities;

namespace GylleneDroppen.Admin.Api.Mappers;

public class WhiskyTastingMapper : IWhiskyTastingMapper
{
    public WhiskyTasting ToWhiskyTasting(CreateWhiskyTastingRequest request)
    {
        throw new NotImplementedException();
    }

    public WhiskyTastingAdminResponse ToWhiskyTastingResponse(WhiskyTasting whiskyTasting)
    {
        throw new NotImplementedException();
    }

    public List<WhiskyTastingAdminResponse> ToWhiskyTastingResponse(List<WhiskyTasting> whiskyTastings)
    {
        throw new NotImplementedException();
    }

    public WhiskyTastingAdminResponse ToWhiskyTastingResponse(UpcomingWhiskyTastingQuery query)
    {
        return new WhiskyTastingAdminResponse
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

    public List<WhiskyTastingAdminResponse> ToWhiskyTastingResponse(List<UpcomingWhiskyTastingQuery> queries)
    {
        return queries.Select(ToWhiskyTastingResponse).ToList();
    }
}