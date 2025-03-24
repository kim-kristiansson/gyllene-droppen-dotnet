namespace GylleneDroppen.Infrastructure.Redis.Models;

public class PasswordResetToken
{
    public string Email { get; set; }
    public string Token { get; set; }
}