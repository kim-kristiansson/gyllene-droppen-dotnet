using System.Security.Claims;
using GylleneDroppen.Application.Interfaces.Services;
using GylleneDroppen.Application.Services;
using Microsoft.AspNetCore.Http;

namespace GylleneDroppen.Infrastructure.Services;

public class CurrentUserService(IHttpContextAccessor httpContextAccessor) : ICurrentUserService
{
    public string? GetUserId()
    {
        return httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
    }

    public string? GetUserEmail()
    {
        return httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Email);
    }

    public bool IsAuthenticated()
    {
        return httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;
    }
}