namespace GylleneDroppen.Admin.Api.Dtos;

public class MessageResponse(string message)
{
    public string Message { get; init; } = message;
}