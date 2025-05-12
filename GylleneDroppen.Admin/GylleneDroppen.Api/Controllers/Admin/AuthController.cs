using GylleneDroppen.Application.Dtos.Auth;
using GylleneDroppen.Application.Interfaces.Shared.Services;
using GylleneDroppen.Presentation.Controllers;
using GylleneDroppen.Presentation.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GylleneDroppen.Api.Controllers.Admin;

[ApiController]
[Route("api/admin/[controller]")]
public class AuthController(IAuthService authService) : BaseAuthController(authService)
{
    private readonly IAuthService _authService = authService;

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var result = await _authService.LoginAsync(request);

        if (!result.IsSuccess) return result.ToActionResult();
        var currentUser = await _authService.GetCurrentUserAsync();
        if (currentUser is { IsSuccess: true, Data.Role: "Admin" }) return result.ToActionResult();
        await _authService.LogoutAsync();
        return Unauthorized(new { message = "You don't have permission to access the admin area." });
    }

    [HttpGet("current-user")]
    public async Task<IActionResult> GetCurrentUser()
    {
        var result = await _authService.GetCurrentUserAsync();

        if (result.IsSuccess && result.Data?.Role != "Admin")
            return Unauthorized(new { message = "You don't have permission to access the user." });

        return result.ToActionResult();
    }
}