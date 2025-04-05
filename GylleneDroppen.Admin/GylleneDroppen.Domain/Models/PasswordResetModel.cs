namespace GylleneDroppen.Domain.Models;

public class PasswordResetModel
{
    public required string Email { get; set; }
    public required string Token { get; set; }
}