using System.Net.Mail;
using GylleneDroppen.Api.Options;
using GylleneDroppen.Core.Interfaces.Repositories;
using GylleneDroppen.Core.Interfaces.Security;
using GylleneDroppen.Core.Interfaces.Services;
using GylleneDroppen.Infrastructure.Data;
using GylleneDroppen.Infrastructure.Repositories;
using GylleneDroppen.Infrastructure.Security;
using GylleneDroppen.Infrastructure.Services;
using GylleneDroppen.Infrastructure.Utilities;
using GylleneDroppen.Shared.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

public static class InfrastructureServiceExtensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        // Database
        services.AddDbContext<AppDbContext>((serviceProvider, options) =>
        {
            var databaseOptions = serviceProvider.GetRequiredService<IOptions<DatabaseOptions>>().Value;
            options.UseNpgsql(databaseOptions.ConnectionString);
        });

        // Repositories
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IAttendeeRepository, AttendeeRepository>();
        services.AddScoped<IRedisRepository, RedisRepository>();

        // Services
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IJwtService, JwtService>();
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<ICookieService, CookieService>();

        // Utilities
        services.AddSingleton<IArgon2Hasher, Argon2Hasher>();

        // Redis
        services.AddStackExchangeRedisCache(options =>
        {
            var redisOptions = configuration.GetSection("RedisOptions").Get<RedisOptions>();
            options.Configuration = redisOptions?.ConnectionString;
            options.InstanceName = redisOptions?.InstanceName;
        });

        // SMTP
        services.AddSingleton<SmtpClient>(serviceProvider =>
        {
            var smtpOptions = serviceProvider.GetRequiredService<IOptions<SmtpOptions>>().Value;
            return SmtpClientFactory.Create(smtpOptions);
        });

        return services;
    }
}