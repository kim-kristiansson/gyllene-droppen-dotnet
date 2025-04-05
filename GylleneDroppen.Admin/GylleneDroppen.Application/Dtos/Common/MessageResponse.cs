namespace GylleneDroppen.Application.Dtos.Common;

public class MessageResponse(string message)
{
    public string Message { get; init; } = message;
}