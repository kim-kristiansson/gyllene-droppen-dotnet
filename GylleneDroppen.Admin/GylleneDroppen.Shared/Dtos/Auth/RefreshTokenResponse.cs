namespace GylleneDroppen.Shared.Dtos.Auth;

public class RefreshTokenResponse
{
    public required string AccessToken { get; init; }
    public required string RefreshToken { get; init; }
}