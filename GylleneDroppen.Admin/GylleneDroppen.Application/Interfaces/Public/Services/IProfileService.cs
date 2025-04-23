using GylleneDroppen.Application.Common.Results;
using GylleneDroppen.Application.Dtos.Public.Profile;

namespace GylleneDroppen.Application.Interfaces.Public.Services;

public interface IProfileService
{
    Task<Result<UserProfileResponse>> GetUserProfileAsync(Guid userId);
    Task<Result<List<UserTastingResponse>>> GetUserTastingsAsync(Guid userId);
}