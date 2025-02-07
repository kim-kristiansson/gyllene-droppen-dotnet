using GylleneDroppen.Admin.Api.Dtos;
using GylleneDroppen.Admin.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GylleneDroppen.Admin.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController(IAuthService authService) : ControllerBase
{
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest loginRequest)
    {
        var response = await authService.LoginAsync(loginRequest);
        return response.ToActionResult();
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest registerRequest)
    {
        var response = await authService.RegisterAsync(registerRequest);
        return response.ToActionResult();
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        var token = Request.Headers.Authorization.ToString().Replace("Bearer ", "");
        var response = await authService.LogoutAsync(token);
        return response.ToActionResult();
    }
}