using GylleneDroppen.Api.Mappers;
using GylleneDroppen.Api.Mappers.Interfaces;
using GylleneDroppen.Api.Repositories;
using GylleneDroppen.Api.Repositories.Interfaces;
using GylleneDroppen.Api.Services;
using GylleneDroppen.Api.Services.Interfaces;
using GylleneDroppen.Api.Utilities;
using GylleneDroppen.Api.Utilities.Interfaces;

namespace GylleneDroppen.Api.Extensions;

public static class DependencyInjectionExtensions
{
    private static readonly Dictionary<Type, Type> Mappers = new()
    {
        { typeof(IEventMapper), typeof(EventMapper) }
    };
    
     private static readonly Dictionary<Type, Type> Services = new()
     {
         { typeof(IAuthService), typeof(AuthService) },
         { typeof(IEmailService), typeof(EmailService) },
         { typeof(IJwtService), typeof(JwtService) },
         { typeof(IEventService), typeof(EventService) },
         { typeof(ICookieService), typeof(CookieService) },
     };
     
     private static readonly Dictionary<Type, Type> Repositories = new()
     {
         { typeof(IUserRepository), typeof(UserRepository) },
         {typeof(IRedisRepository), typeof(RedisRepository)},
         {typeof(IEventRepository), typeof(EventRepository)},
         {typeof(IParticipantRepository), typeof(ParticipantRepository)},
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
        services.AddSingleton(Mappers);
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