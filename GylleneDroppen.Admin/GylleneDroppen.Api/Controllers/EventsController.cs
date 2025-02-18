using System.Security.Claims;
using GylleneDroppen.Api.Attributes;
using GylleneDroppen.Api.Dtos;
using GylleneDroppen.Api.Dtos.Event;
using GylleneDroppen.Api.Extensions;
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

    [Admin]
    [HttpPost("all")]
    public async Task<IActionResult> GetAllEvents()
    {
        var response = await eventService.GetAllEventsAsync();
        return response.ToActionResult();
    }
    
    [Authorize]
    [HttpGet("upcoming")]
    public async Task<IActionResult> GetUpcomingEvents()
    {
        var response = await eventService.GetUpcomingEventsAsync();
        return response.ToActionResult();
    }

    [Authorize]
    [HttpPost("register")]
    public async Task<IActionResult> RegisterForEvent([FromBody] RegisterForEventRequest request)
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var response = await eventService.RegisterForEventAsync(request, userId);
        return response.ToActionResult();
    }

    [Admin]
    [HttpPut("update")]
    public async Task<IActionResult> UpdateEvent([FromBody] UpdateEventRequest eventRequest)
    {
        var response = await eventService.UpdateEventAsync(eventRequest);
        return response.ToActionResult();
    }
}