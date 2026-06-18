using System.Security.Cryptography;
using System.Text;

namespace DeveloperRegistry.Api.Common.Security;

public static class ApiKeyGenerator
{
    public static string GeneratePlainTextKey()
    {
        var bytes = RandomNumberGenerator.GetBytes(32);
        return Convert.ToHexString(bytes).ToLowerInvariant();
    }

    public static string Hash(string key)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(key));
        return Convert.ToHexString(bytes).ToLowerInvariant();
    }
}
