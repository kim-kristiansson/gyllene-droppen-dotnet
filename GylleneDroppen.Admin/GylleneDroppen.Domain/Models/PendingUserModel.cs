using GylleneDroppen.Domain.Entities;

namespace GylleneDroppen.Domain.Models;

public class PendingUserModel
{
    public required User User { get; init; }
    public required string ConfirmationCode { get; init; }
}