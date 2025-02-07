using GylleneDroppen.Admin.Api.Utilities.Interfaces;

namespace GylleneDroppen.Admin.Api.Middlewares;

public class JwtBlacklistMiddleware(RequestDelegate next, IJsonWebToken jsonWebToken)
{
    public async Task InvokeAsync(HttpContext context)
    {
        var token = context.Request.Headers.Authorization.FirstOrDefault()?.Split(" ").Last();

        if (!string.IsNullOrEmpty(token) && await jsonWebToken.IsTokenBlacklistedAsync(token))
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync("Session expired. Please log in again.");
            return;
        }

        await next(context);
    }
}