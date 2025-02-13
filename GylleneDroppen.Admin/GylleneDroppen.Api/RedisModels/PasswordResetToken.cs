namespace GylleneDroppen.Api.RedisModels;

public class PasswordResetToken
{
    public string Email { get; set; }
    public string Token { get; set; }
}