namespace GylleneDroppen.Admin.Api.Services.Interfaces;

public interface IArgon2Hasher
{
    (string Hash, string Salt) HashPassword(string password);
    bool VerifyPassword(string password, string storedHash, string storedSalt);
} 