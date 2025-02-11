using GylleneDroppen.Api.Repositories;
using GylleneDroppen.Api.Repositories.Interfaces;
using GylleneDroppen.Api.Services;
using GylleneDroppen.Api.Services.Interfaces;
using GylleneDroppen.Api.Utilities;
using GylleneDroppen.Api.Utilities.Interfaces;

namespace GylleneDroppen.Api.Extensions;

public static class DependencyInjectionExtensions
{
     private static readonly Dictionary<Type, Type> Services = new()
     {
         { typeof(IAuthService), typeof(AuthService) },
         { typeof(IJwtService), typeof(JwtService) }
     };
     
     private static readonly Dictionary<Type, Type> Repositories = new()
     {
         { typeof(IUserRepository), typeof(UserRepository) },
         {typeof(IRedisRepository), typeof(RedisRepository)}
     };
     
     private static readonly Dictionary<Type, Type> Utilities = new()
     {
         { typeof(IArgon2Hasher), typeof(Argon2Hasher) }
     };

    
    public static void AddDependencyInjections(this IServiceCollection services)
    {
        services.AddScoped(Services);
        services.AddScoped(Repositories);
        services.AddSingleton(Utilities);
    }
    
    private static void AddScoped(this IServiceCollection services, Dictionary<Type, Type> mappings)
    {
        foreach (var mapping in mappings)
        {
            services.AddScoped(mapping.Key, mapping.Value);
        }
    }
    
    private static void AddSingleton(this IServiceCollection services, Dictionary<Type, Type> mappings)
    {
        foreach (var mapping in mappings)
        {
            services.AddSingleton(mapping.Key, mapping.Value);
        }
    }
}