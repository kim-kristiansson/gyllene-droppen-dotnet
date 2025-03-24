namespace GylleneDroppen.Shared.Dtos.Auth;

public class RefreshTokenRequest
{
    public required Guid UserId { get; init; }
    public required string RefreshToken { get; init; }
}