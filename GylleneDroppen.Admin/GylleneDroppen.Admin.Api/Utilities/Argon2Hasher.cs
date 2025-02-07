using System.Security.Cryptography;
using System.Text;
using GylleneDroppen.Admin.Api.Services.Interfaces;
using GylleneDroppen.Admin.Api.Utilities.Interfaces;
using Konscious.Security.Cryptography;

namespace GylleneDroppen.Admin.Api.Utilities;

public class Argon2Hasher : IArgon2Hasher
{
    private const int SaltSize = 16;
    private const int HashSize = 32;
    private const int Iterations = 4;
    private const int MemorySize = 65536;
    private const int Parallelism = 2;
    
    public (string Hash, string Salt) HashPassword(string password)
    {
        var salt = RandomNumberGenerator.GetBytes(SaltSize);
        var hash = HashArgon2(password, salt);
        
        return (Convert.ToBase64String(hash), Convert.ToBase64String(salt));
    }

    public bool VerifyPassword(string password, string storedHash, string storedSalt)
    {
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