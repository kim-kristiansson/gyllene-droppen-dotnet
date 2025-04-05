namespace GylleneDroppen.Application.Dtos.Email;

public class ConfirmEmailRequest
{
    public required string Email { get; init; }
    public required string ConfirmationCode { get; init; }
}