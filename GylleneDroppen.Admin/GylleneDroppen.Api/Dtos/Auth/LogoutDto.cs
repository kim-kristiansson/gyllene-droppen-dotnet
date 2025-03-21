namespace GylleneDroppen.Api.Dtos;

public class LogoutRequest
{
    public required Guid UserId { get; init; }
    public required string RefreshToken { get; init; }
}