namespace GylleneDroppen.Core.Dtos.Auth;

public class LogoutRequest
{
    public required Guid UserId { get; init; }
    public required string RefreshToken { get; init; }
}