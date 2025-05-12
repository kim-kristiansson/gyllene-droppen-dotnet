using GylleneDroppen.Admin.Application.Dtos.Admin;
using GylleneDroppen.Admin.Application.Interfaces.Services;
using GylleneDroppen.Presentation.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace GylleneDroppen.Api.Controllers.Admin;

[ApiController]
[Route("api/admin/[controller]")]
public class AdminController(IAdminService adminService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAllAdmins()
    {
        var result = await adminService.GetAllAdminsAsync();
        return result.ToActionResult();
    }

    [HttpPost("promote/{userId:guid}")]
    public async Task<IActionResult> PromoteUserToAdmin(Guid userId)
    {
        var result = await adminService.PromoteUserToAdminAsync(userId);
        return result.ToActionResult();
    }

    [HttpPost("demote")]
    public async Task<IActionResult> DemoteAdmin([FromBody] DemoteAdminRequest request)
    {
        var result = await adminService.DemoteUserToAdminAsync(request);
        return result.ToActionResult();
    }
}