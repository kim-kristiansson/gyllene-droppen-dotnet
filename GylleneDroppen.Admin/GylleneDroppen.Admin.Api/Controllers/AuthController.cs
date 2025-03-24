using System.Security.Claims;
using GylleneDroppen.Core.Enums;
using GylleneDroppen.Core.Interfaces.Services;
using GylleneDroppen.Shared.Dtos.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GylleneDroppen.Admin.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController(IAuthService authService) : ControllerBase
{
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var response = await authService.LoginAsync(request);

        if (response.IsSuccess && response.Data?.Role != RoleType.Admin.ToString())
            return Unauthorized(new { message = "Administrator access required" });

        return response.ToActionResult();
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        var response = await authService.LogoutAsync();
        return response.ToActionResult();
    }

    [Authorize]
    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken()
    {
        var response = await authService.RefreshTokenAsync();
        return response.ToActionResult();
    }

    [HttpPost("confirm-email")]
    public async Task<IActionResult> ConfirmEmail([FromBody] ConfirmEmailRequest request)
    {
        var result = await authService.ConfirmEmailAsync(request);
        return result.ToActionResult();
    }

    [Authorize]
    [HttpGet("me")]
    public IActionResult GetCurrentUser()
    {
        if (User.Identity is not ClaimsIdentity claimsIdentity)
            return Unauthorized("Invalid token.");

        var claims = claimsIdentity.Claims
            .Select(c => new Claim(c.Type, c.Value))
            .ToList();

        return Ok(claims);
    }
}