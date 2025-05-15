using System.Security.Cryptography;

namespace GylleneDroppen.Application.Utilities;

public static class CodeGenerator
{
    public static string GenerateSecureToken()
    {
        var tokenBytes = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(tokenBytes);

        var base64Token = Convert.ToBase64String(tokenBytes);
        return MakeBase64UrlSafe(base64Token);
    }

    private static string MakeBase64UrlSafe(string base64)
    {
        return base64
            .Replace('+', '-')
            .Replace('/', '_')
            .TrimEnd('=');
    }
}