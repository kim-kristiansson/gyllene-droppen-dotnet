namespace GylleneDroppen.Shared.Dtos.Generic;

public class MessageResponse(string message)
{
    public string Message { get; init; } = message;
}