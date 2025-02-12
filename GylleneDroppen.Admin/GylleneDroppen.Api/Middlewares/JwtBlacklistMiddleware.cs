using GylleneDroppen.Api.Services.Interfaces;

namespace GylleneDroppen.Api.Middlewares;

public class JwtBlacklistMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context, IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var jwtService = scope.ServiceProvider.GetService<IJwtService>();

        if (jwtService != null)
        {
            await next(context);
            return;
        }
        
        var authorizationHeader = context.Request.Headers.Authorization.FirstOrDefault();
        if (string.IsNullOrWhiteSpace(authorizationHeader) || !authorizationHeader.StartsWith("Bearer "))
        {
            await next(context);
            return;
        }
        
        var token = authorizationHeader.Split(" ").Last();
        
        if (!string.IsNullOrEmpty(token) && await jwtService?.IsTokenBlacklistedAsync(token)!)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync("Session expired. Please log in again.");
            return;
        }

        await next(context);
    }
}