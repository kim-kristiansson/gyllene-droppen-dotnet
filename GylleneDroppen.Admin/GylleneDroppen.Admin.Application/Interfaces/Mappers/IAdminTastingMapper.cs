using GylleneDroppen.Application.Dtos.Tasting;
using GylleneDroppen.Domain.Entities;

namespace GylleneDroppen.Admin.Application.Interfaces.Mappers;

public interface IAdminTastingMapper
{
    Tasting ToTasting(CreateTastingRequest request);
    TastingAdminResponse ToTastingAdminResponse(Tasting tasting);
    TastingUserResponse ToTastingUserResponse(Tasting tasting);
    List<TastingUserResponse> ToTastingUserResponse(List<Tasting> tastings);
    List<TastingAdminResponse> ToTastingAdminResponse(List<Tasting> tastings);
}