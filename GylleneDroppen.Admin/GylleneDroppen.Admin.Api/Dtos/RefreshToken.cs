namespace GylleneDroppen.Admin.Api.Dtos;

public class RefreshTokenRequest
{
    public required Guid UserId { get; init; }
    public required string RefreshToken { get; init; }
}

public class RefreshTokenResponse
{
    public required string AccessToken { get; init; }
    public required string RefreshToken { get; init; }
}