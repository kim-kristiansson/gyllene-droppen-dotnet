using GylleneDroppen.Api.Attributes;
using GylleneDroppen.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GylleneDroppen.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AdminsController(IAdminService adminService) : ControllerBase
{
    [Admin]
    [HttpPost("promote/{userId:guid}")]
    public async Task<IActionResult> Promote(Guid userId)
    {
        var response = await adminService.PromoteUserToAdminAsync(userId);
        return response.ToActionResult();
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var response = await adminService.GetAllAdminsAsync();
        return response.ToActionResult();
    }
}