using System.Security.Claims;
using GylleneDroppen.Application.Interfaces.Public.Services;
using GylleneDroppen.Presentation.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GylleneDroppen.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ProfileController(IProfileService profileService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetProfile()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
            return Unauthorized();

        var result = await profileService.GetUserProfileAsync(userId);
        return result.ToActionResult();
    }

    [HttpGet("tastings")]
    public async Task<IActionResult> GetUserTastings()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
            return Unauthorized();

        var result = await profileService.GetUserTastingsAsync(userId);
        return result.ToActionResult();
    }
}