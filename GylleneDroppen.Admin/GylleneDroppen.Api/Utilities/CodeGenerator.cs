using System.Security.Cryptography;

namespace GylleneDroppen.Api.Utilities;

public static class CodeGenerator
{
    public static string GenerateVerificationCode(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        return new string(Enumerable.Range(0, length)
            .Select(_ => chars[RandomNumberGenerator.GetInt32(0, chars.Length)])
            .ToArray());
    }
}