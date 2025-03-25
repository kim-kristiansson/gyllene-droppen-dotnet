namespace GylleneDroppen.Core.Dtos.Auth;

public class RefreshTokenResponse
{
    public required string AccessToken { get; init; }
    public required string RefreshToken { get; init; }
}