namespace GylleneDroppen.Core.Dtos.Generic;

public class MessageResponse(string message)
{
    public string Message { get; init; } = message;
}