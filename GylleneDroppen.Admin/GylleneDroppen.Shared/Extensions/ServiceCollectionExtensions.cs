using GylleneDroppen.Core.Interfaces.Repositories;
using GylleneDroppen.Core.Interfaces.Security;
using GylleneDroppen.Core.Interfaces.Services;
using GylleneDroppen.Infrastructure.Repositories;
using GylleneDroppen.Infrastructure.Security;
using GylleneDroppen.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace GylleneDroppen.Shared.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddSharedApiServices(this IServiceCollection services)
    {
        // Services
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<IJwtService, JwtService>();
        services.AddScoped<ICookieService, CookieService>();

        // Repositories
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRedisRepository, RedisRepository>();
        services.AddScoped<IAttendeeRepository, AttendeeRepository>();

        // Utilities
        services.AddScoped<IArgon2Hasher, Argon2Hasher>();
    }
}