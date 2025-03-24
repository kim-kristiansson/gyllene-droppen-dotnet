namespace GylleneDroppen.Api.Options;

public class RedisOptions
{
    public required string ConnectionString { get; init; }
    public required string InstanceName { get; init; }
}