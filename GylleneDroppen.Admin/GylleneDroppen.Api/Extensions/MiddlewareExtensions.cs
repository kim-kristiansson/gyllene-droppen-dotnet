using GylleneDroppen.Api.Middlewares;

namespace GylleneDroppen.Api.Extensions;

public static class MiddlewareExtensions
{
    private static readonly List<Type> Middlewares = [
        typeof(JwtBlacklistMiddleware)
    ];

    public static void UseMiddlewares(this IApplicationBuilder app)
    {
        foreach (var middleware in Middlewares)
        {
            app.UseMiddleware(middleware);
        }
    }
}