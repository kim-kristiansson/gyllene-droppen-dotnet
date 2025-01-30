namespace GylleneDroppen.Admin.Api.Options;

public class JwtOptions
{
    public required string Secret { get; init; }
    public required string Issuer { get; init; }
    public required string Audience { get; init; }
    public required int ExpirationMinutes { get; init; }
}