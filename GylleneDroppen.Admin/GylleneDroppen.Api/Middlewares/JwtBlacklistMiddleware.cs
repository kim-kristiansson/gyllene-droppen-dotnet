using GylleneDroppen.Api.Services.Interfaces;

namespace GylleneDroppen.Api.Middlewares;

public class JwtBlacklistMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context, IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var jwtService = scope.ServiceProvider.GetService<IJwtService>();
        var token = context.Request.Headers.Authorization.FirstOrDefault()?.Split(" ").Last();
        if (!string.IsNullOrEmpty(token) && await jwtService?.IsTokenBlacklistedAsync(token)!)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync("Session expired. Please log in again.");
            return;
        }

        await next(context);
    }
}