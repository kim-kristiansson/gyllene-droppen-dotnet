using GylleneDroppen.Application.Dtos.Tasting;
using GylleneDroppen.Application.Interfaces.Admin.Services;
using GylleneDroppen.Presentation.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace GylleneDroppen.Admin.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TastingsController(IAdminTastingService tastingService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAllTastings()
    {
        var result = await tastingService.GetAllTastingsAsync();
        return result.ToActionResult();
    }

    [HttpGet("upcoming")]
    public async Task<IActionResult> GetUpcomingTastings()
    {
        var result = await tastingService.GetAllUpcomingTastingsAsync();
        return result.ToActionResult();
    }

    [HttpPost]
    public async Task<IActionResult> CreateTasting(CreateTastingRequest request)
    {
        var result = await tastingService.CreateTastingAsync(request);
        return result.ToActionResult();
    }

    [HttpPut]
    public async Task<IActionResult> UpdateTasting(UpdateTastingRequest request)
    {
        var result = await tastingService.UpdateTastingAsync(request);
        return result.ToActionResult();
    }
}