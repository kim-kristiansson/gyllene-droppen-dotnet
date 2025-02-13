using GylleneDroppen.Api.Attributes;
using GylleneDroppen.Api.Dtos;
using GylleneDroppen.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GylleneDroppen.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EventsController(IEventService eventService) : ControllerBase
{
    [Admin]
    [HttpPost("create")]
    public async Task<IActionResult> CreateEvent([FromBody] CreateEventRequest request)
    {
        var response = await eventService.CreateEventAsync(request);
        return response.ToActionResult();
    }
    
    [Authorize]
    [HttpGet("upcoming")]
    public async Task<IActionResult> GetUpcomingEvents()
    {
        var response = await eventService.GetUpcomingEvents();
        return response.ToActionResult();
    }
}