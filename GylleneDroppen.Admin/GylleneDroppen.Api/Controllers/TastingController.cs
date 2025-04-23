using System.Security.Claims;
using GylleneDroppen.Application.Dtos.Tasting;
using GylleneDroppen.Application.Interfaces.Public.Services;
using GylleneDroppen.Presentation.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GylleneDroppen.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TastingsController(ITastingService tastingService) : ControllerBase
{
    [AllowAnonymous]
    [HttpGet("upcoming")]
    public async Task<IActionResult> GetUpcomingTastings()
    {
        var result = await tastingService.GetUpcomingTastingsAsync();
        return result.ToActionResult();
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetTastingById(Guid id)
    {
        var result = await tastingService.GetTastingByIdAsync(id);
        return result.ToActionResult();
    }

    [Authorize]
    [HttpPost("register")]
    public async Task<IActionResult> RegisterForTasting([FromBody] RegisterForTastingRequest request)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
            return Unauthorized();

        var result = await tastingService.RegisterForTastingAsync(userId, request);
        return result.ToActionResult();
    }
}