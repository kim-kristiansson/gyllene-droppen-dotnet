using GylleneDroppen.Api.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GylleneDroppen.Infrastructure.Redis.Extensions;

public static class RedisExtensions
{
    public static void AddRedis(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<RedisOptions>(configuration.GetSection("RedisOptions"));

        services.AddStackExchangeRedisCache(options =>
        {
            var redisOptions = configuration.GetSection("RedisOptions").Get<RedisOptions>();
            options.Configuration = redisOptions?.ConnectionString;
            options.InstanceName = redisOptions?.InstanceName;
        });
    }
}