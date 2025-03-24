using GylleneDroppen.Admin.Api.Dtos.WhiskyTasting;
using GylleneDroppen.Admin.Api.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace GylleneDroppen.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class WhiskyTastingsController(IWhiskyTastingService whiskyTastingService) : ControllerBase
{
    [HttpPost("create")]
    public async Task<IActionResult> CreateEvent([FromBody] CreateWhiskyTastingRequest request)
    {
        var response = await whiskyTastingService.CreateWhiskyTastingAsync(request);
        return response.ToActionResult();
    }

    [HttpPost("all")]
    public async Task<IActionResult> GetAllEvents()
    {
        var response = await whiskyTastingService.GetAllWhiskyTastingsAsync();
        return response.ToActionResult();
    }

    [HttpGet("upcoming")]
    public async Task<IActionResult> GetUpcomingEvents()
    {
        var response = await whiskyTastingService.GetUpcomingWhiskyTastingsAsync();
        return response.ToActionResult();
    }

    [HttpPut("update")]
    public async Task<IActionResult> UpdateEvent([FromBody] UpdateWhiskyTastingRequest request)
    {
        var response = await whiskyTastingService.UpdateWhiskyTastingAsync(request);
        return response.ToActionResult();
    }
}