namespace GylleneDroppen.Application.Dtos.Shared.Auth;

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