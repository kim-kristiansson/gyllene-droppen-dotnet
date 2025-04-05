using GylleneDroppen.Application.Dtos.Tasting;
using GylleneDroppen.Application.Interfaces.Mappers.Admin;
using GylleneDroppen.Application.Queries;
using GylleneDroppen.Domain.Entities;

namespace GylleneDroppen.Presentation.Mappers;

public class TastingMapper : ITastingMapper
{
    public Tasting ToTasting(CreateTastingRequest request)
    {
        return new Tasting
        {
            Id = Guid.NewGuid(),
            Title = request.Title,
            Description = request.Description,
            Location = request.Location,
            StartTime = request.StartTime,
            EndTime = request.EndTime,
            Capacity = request.Capacity,
            CreatedAt = DateTime.UtcNow,
            Price = request.Price,
            Deadline = request.Deadline,
            OrganizerId = request.OrganizerId,
            CreatedById = request.CreatedById,
            Attendees = []
        };
    }

    public TastingAdminResponse ToTastingAdminResponse(Tasting tasting)
    {
        return new TastingAdminResponse
        {
            Id = tasting.Id,
            Title = tasting.Title,
            Description = tasting.Description,
            Location = tasting.Location,
            StartTime = tasting.StartTime,
            EndTime = tasting.EndTime,
            Capacity = tasting.Capacity,
            CreatedAt = tasting.CreatedAt,
            Price = tasting.Price,
            Deadline = tasting.Deadline,
            OrganizerId = tasting.OrganizerId,
            CreatedById = tasting.CreatedById
        };
    }

    public TastingUserResponse ToTastingUserResponse(Tasting tasting)
    {
        return new TastingUserResponse
        {
            Id = tasting.Id,
            Title = tasting.Title,
            Description = tasting.Description,
            StartTime = tasting.StartTime,
            EndTime = tasting.EndTime,
            Location = tasting.Location,
            Capacity = tasting.Capacity,
            Price = tasting.Price,
            Deadline = tasting.Deadline
        };
    }

    public TastingUserResponse ToTastingUserResponse(UpcomingTastingsQuery tasting)
    {
        return new TastingUserResponse
        {
            Id = tasting.Id,
            Title = tasting.Title,
            Description = tasting.Description,
            StartTime = tasting.StartTime,
            EndTime = tasting.EndTime,
            Location = tasting.Location,
            Capacity = tasting.Capacity,
            Price = tasting.Price,
            Deadline = tasting.Deadline
        };
    }

    public List<TastingUserResponse> ToTastingUserResponse(List<Tasting> tastings)
    {
        return tastings.Select(ToTastingUserResponse).ToList();
    }

    public List<TastingAdminResponse> ToTastingAdminResponse(List<Tasting> tastings)
    {
        return tastings.Select(ToTastingAdminResponse).ToList();
    }

    public List<TastingUserResponse> ToTastingUserResponse(List<UpcomingTastingsQuery> tastings)
    {
        return tastings.Select(ToTastingUserResponse).ToList();
    }
}