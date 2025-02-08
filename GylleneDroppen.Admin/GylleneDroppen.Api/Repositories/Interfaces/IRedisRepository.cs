namespace GylleneDroppen.Api.Repositories.Interfaces;

public interface IRedisRepository
{ 
    Task SaveAsync(string key, string value, TimeSpan ttl, string prefix = "");
    Task<string?> GetAsync(string key, string prefix = "");
    Task<bool> DeleteAsync(string key, string prefix = "");
    Task<bool> ExistsAsync(string key, string prefix = "");
}