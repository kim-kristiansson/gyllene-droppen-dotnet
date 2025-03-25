using GylleneDroppen.Api.Dtos.WhiskyTasting;
using GylleneDroppen.Api.Interfaces.Mappers;
using GylleneDroppen.Api.Queries.WhiskyTasting;
using GylleneDroppen.Core.Dtos.Generic;

namespace GylleneDroppen.Api.Mappers;

public class WhiskyTastingMapper : IWhiskyTastingMapper
{
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