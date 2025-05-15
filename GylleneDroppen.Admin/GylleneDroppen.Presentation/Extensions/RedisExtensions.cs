using GylleneDroppen.Infrastructure.Settings;

namespace GylleneDroppen.Presentation.Extensions;

public static class RedisExtensions
{
    public static void AddRedis(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<RedisSettings>(configuration.GetSection("RedisSettings"));

        services.AddStackExchangeRedisCache(options =>
        {
            var redisSettings = configuration.GetSection("RedisSettings").Get<RedisSettings>();
            options.Configuration = redisSettings?.ConnectionString;
            options.InstanceName = redisSettings?.InstanceName;
        });
    }
}