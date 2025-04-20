using GylleneDroppen.Application.Interfaces.Shared.Services;
using GylleneDroppen.Presentation.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace GylleneDroppen.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BaseAuthController(IAuthService authService) : ControllerBase
{
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        var result = await authService.LogoutAsync();
        return result.ToActionResult();
    }

    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken()
    {
        var result = await authService.RefreshTokenAsync();
        return result.ToActionResult();
    }
}