using System.Security.Cryptography;
using System.Text;
using GylleneDroppen.Application.Interfaces.Security;
using Isopoh.Cryptography.Argon2;

namespace GylleneDroppen.Infrastructure.Security;

public class Argon2PasswordHasher : IPasswordHasher
{
    public string Hash(string password)
    {
        var config = new Argon2Config
        {
            Type = Argon2Type.DataDependentAddressing, // Argon2d (or use Argon2id for hybrid)
            Version = Argon2Version.Nineteen,
            TimeCost = 4,
            MemoryCost = 65536, // 64 MB
            Lanes = 2,
            Threads = Environment.ProcessorCount,
            Password = Encoding.UTF8.GetBytes(password),
            Salt = RandomNumberGenerator.GetBytes(16),
            HashLength = 32
        };

        using var hasher = new Argon2(config);
        using var hash = hasher.Hash();

        return config.EncodeString(hash.Buffer);
    }

    public bool Verify(string hash, string password)
    {
        return Argon2.Verify(hash, password);
    }
}