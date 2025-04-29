using GylleneDroppen.Application.Dtos.Public.Tasting;
using GylleneDroppen.Domain.Entities;

namespace GylleneDroppen.Application.Interfaces.Public.Mappers;

public interface ITastingMapper
{
    TastingResponse ToTastingResponse(Tasting tasting);
    List<TastingResponse> ToTastingResponse(List<Tasting> tastings);
}