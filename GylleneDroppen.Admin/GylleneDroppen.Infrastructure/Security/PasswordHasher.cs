using System.Security.Cryptography;
using System.Text;
using GylleneDroppen.Application.Interfaces.Security.Cryptography;
using Konscious.Security.Cryptography;

namespace GylleneDroppen.Infrastructure.Security;

public class PasswordHasher : IPasswordHasher
{
    private const int SaltSize = 16;
    private const int HashSize = 32;
    private const int Iterations = 4;
    private const int MemorySize = 65536;
    private const int Parallelism = 2;

    public (string Hash, string Salt) HashPassword(string? password)
    {
        if (string.IsNullOrWhiteSpace(password))
            throw new ArgumentException("Password cannot be null or empty", nameof(password));

        var salt = RandomNumberGenerator.GetBytes(SaltSize);
        var hash = HashArgon2(password, salt);

        return (Convert.ToBase64String(hash), Convert.ToBase64String(salt));
    }

    public bool VerifyPassword(string? password, string? storedHash, string? storedSalt)
    {
        if (string.IsNullOrWhiteSpace(password))
            throw new ArgumentException("Password cannot be null or empty", nameof(password));

        if (string.IsNullOrWhiteSpace(storedHash))
            throw new ArgumentException("Stored hash cannot be null or empty", nameof(storedHash));

        if (string.IsNullOrWhiteSpace(storedSalt))
            throw new ArgumentException("Stored salt cannot be null or empty", nameof(storedSalt));

        var saltBytes = Convert.FromBase64String(storedSalt);
        var computedHash = HashArgon2(password, saltBytes);

        return Convert.ToBase64String(computedHash) == storedHash;
    }

    private static byte[] HashArgon2(string password, byte[] salt)
    {
        using var argon2 = new Argon2id(Encoding.UTF8.GetBytes(password))
        {
            Salt = salt,
            DegreeOfParallelism = Parallelism,
            MemorySize = MemorySize,
            Iterations = Iterations
        };

        return argon2.GetBytes(HashSize);
    }
}