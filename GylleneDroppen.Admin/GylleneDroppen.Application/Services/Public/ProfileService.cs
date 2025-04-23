using GylleneDroppen.Application.Common.Results;
using GylleneDroppen.Application.Dtos.Public.Profile;
using GylleneDroppen.Application.Interfaces.Public.Services;
using GylleneDroppen.Application.Interfaces.Shared.Repositories;

namespace GylleneDroppen.Application.Services.Public;

public class ProfileService(
    IUserRepository userRepository,
    IAttendeeRepository attendeeRepository,
    ITastingRepository tastingRepository)
    : IProfileService
{
    public async Task<Result<UserProfileResponse>> GetUserProfileAsync(Guid userId)
    {
        var user = await userRepository.GetByIdAsync(userId);
        if (user == null)
            return Result<UserProfileResponse>.Failure("User not found.", 404);

        var profile = new UserProfileResponse
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            CreatedAt = user.CreatedAt,
            Role = user.Role.ToString()
        };

        return Result<UserProfileResponse>.Success(profile);
    }

    public async Task<Result<List<UserTastingResponse>>> GetUserTastingsAsync(Guid userId)
    {
        var registrations = await attendeeRepository.FindAsync(a => a.UserId == userId);

        if (registrations.Count == 0)
            return Result<List<UserTastingResponse>>.Success(new List<UserTastingResponse>());

        var tastingIds = registrations.Select(r => r.TastingId).ToList();
        var userTastings = new List<UserTastingResponse>();

        foreach (var tastingId in tastingIds)
        {
            var tasting = await tastingRepository.GetByIdAsync(tastingId);
            if (tasting == null) continue;
            var registration = registrations.First(r => r.TastingId == tastingId);

            userTastings.Add(new UserTastingResponse
            {
                TastingId = tasting.Id,
                Title = tasting.Title,
                Description = tasting.Description,
                StartTime = tasting.StartTime,
                EndTime = tasting.EndTime,
                Location = tasting.Location,
                RegisteredAt = registration.RegisteredAt,
                Status = tasting.StartTime > DateTime.UtcNow ? "Upcoming" : "Past"
            });
        }

        return Result<List<UserTastingResponse>>.Success(userTastings);
    }
}