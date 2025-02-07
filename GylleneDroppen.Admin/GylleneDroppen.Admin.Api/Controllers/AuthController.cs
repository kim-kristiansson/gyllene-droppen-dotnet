using GylleneDroppen.Admin.Api.Dtos;
using GylleneDroppen.Admin.Api.Services.Interfaces;
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
        return response.ToActionResult();
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        var response = await authService.RegisterAsync(request);
        return response.ToActionResult();
    }

    [Authorize]
    [HttpPost("logout")]
    public async Task<IActionResult> Logout(LogoutRequest request)
    {
        var accessToken = Request.Headers.Authorization.ToString().Replace("Bearer ", "");
        
        var response = await authService.LogoutAsync(accessToken, request);
        return response.ToActionResult();
    }

    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken(RefreshTokenRequest request)
    {
        var response = await authService.RefreshTokenAsync(request);
        return response.ToActionResult();
    }
}