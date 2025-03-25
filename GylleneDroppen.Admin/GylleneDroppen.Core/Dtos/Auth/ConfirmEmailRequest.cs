namespace GylleneDroppen.Core.Dtos.Auth;

public class ConfirmEmailRequest
{
    public required string Email { get; init; }
    public required string ConfirmationCode { get; init; }
}