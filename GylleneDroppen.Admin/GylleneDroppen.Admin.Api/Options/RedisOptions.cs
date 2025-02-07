namespace GylleneDroppen.Admin.Api.Options;

public class RedisOptions
{
    public string ConnectionString { get; set; }
    public string BlacklistedTokenTtl { get; set; }
}