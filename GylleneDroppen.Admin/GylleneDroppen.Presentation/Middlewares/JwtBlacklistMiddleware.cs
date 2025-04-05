using GylleneDroppen.Application.Services.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace GylleneDroppen.Presentation.Middlewares;

public class JwtBlacklistMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context, IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var jwtService = scope.ServiceProvider.GetRequiredService<IJwtService>();

        var token = context.Request.Cookies["accessToken"];

        if (!string.IsNullOrWhiteSpace(token))
        {
            var isBlacklisted = await jwtService.IsAccessTokenBlacklistedAsync(token);

            if (isBlacklisted)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Session expired. Please log in again.");
                return;
            }
        }

        // Move to the next middleware in the pipeline
        await next(context);
    }
}