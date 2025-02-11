namespace GylleneDroppen.Api.Dtos;

public class ConfirmEmailRequest
{
    public required string Email { get; init; }
    public required string ConfirmationCode { get; init; }
}