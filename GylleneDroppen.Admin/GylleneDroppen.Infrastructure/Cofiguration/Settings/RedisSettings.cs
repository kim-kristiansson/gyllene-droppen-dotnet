namespace GylleneDroppen.Infrastructure.Cofiguration.Settings;

public class RedisSettings
{
    public required string ConnectionString { get; init; }
    public required string InstanceName { get; init; }
}