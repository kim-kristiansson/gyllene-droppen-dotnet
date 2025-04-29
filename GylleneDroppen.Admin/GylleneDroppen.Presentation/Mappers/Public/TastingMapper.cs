using GylleneDroppen.Application.Dtos.Public.Tasting;
using GylleneDroppen.Application.Interfaces.Public.Mappers;
using GylleneDroppen.Domain.Entities;

namespace GylleneDroppen.Presentation.Mappers.Public;

public class TastingMapper : ITastingMapper
{
    public TastingResponse ToTastingResponse(Tasting tasting)
    {
        return new TastingResponse
        {
            Id = tasting.Id,
            Description = tasting.Description,
            Location = tasting.Location,
            Title = tasting.Title,
            StartTime = tasting.StartTime,
            EndTime = tasting.EndTime,
            Capacity = tasting.Capacity,
            Price = tasting.Price,
            Deadline = tasting.Deadline
        };
    }

    public List<TastingResponse> ToTastingResponse(List<Tasting> tastings)
    {
        return tastings.Select(ToTastingResponse).ToList();
    }
}