using System.Security.Cryptography;

namespace GylleneDroppen.Application.Utilities;

public static class CodeGenerator
{
    public static string GenerateConfirmationCode(int length)
    {
        if (length <= 0)
            return string.Empty;

        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        return new string(Enumerable.Range(0, length)
            .Select(_ => chars[RandomNumberGenerator.GetInt32(0, chars.Length)])
            .ToArray());
    }
}