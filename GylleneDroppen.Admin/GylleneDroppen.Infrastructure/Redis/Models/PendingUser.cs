using GylleneDroppen.Core.Entities;

namespace GylleneDroppen.Infrastructure.Redis.Models;

public class PendingUser
{
    public required User User { get; init; }
    public required string ConfirmationCode { get; init; }
}