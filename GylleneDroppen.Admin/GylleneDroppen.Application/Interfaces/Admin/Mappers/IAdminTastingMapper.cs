using GylleneDroppen.Application.Dtos.Tasting;
using GylleneDroppen.Application.Queries;
using GylleneDroppen.Domain.Entities;

namespace GylleneDroppen.Application.Interfaces.Admin.Mappers;

public interface IAdminTastingMapper
{
    Tasting ToTasting(CreateTastingRequest request);
    TastingAdminResponse ToTastingAdminResponse(Tasting tasting);
    TastingUserResponse ToTastingUserResponse(Tasting tasting);
    TastingUserResponse ToTastingUserResponse(UpcomingTastingsQuery tasting);
    List<TastingUserResponse> ToTastingUserResponse(List<Tasting> tastings);
    List<TastingAdminResponse> ToTastingAdminResponse(List<Tasting> tastings);
    List<TastingUserResponse> ToTastingUserResponse(List<UpcomingTastingsQuery> tastings);
}