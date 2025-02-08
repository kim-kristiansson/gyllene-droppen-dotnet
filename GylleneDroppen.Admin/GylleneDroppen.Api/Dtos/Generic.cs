namespace GylleneDroppen.Api.Dtos;

public class MessageResponse(string message)
{
    public string Message { get; init; } = message;
}