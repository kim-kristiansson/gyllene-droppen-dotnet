using GylleneDroppen.Api.Models;

namespace GylleneDroppen.Api.RedisModels;

public class PendingUser
{
    public required User User { get; init; }
    public required string ConfirmationCode { get; init; }
}