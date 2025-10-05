using System.Security.Cryptography;
using System.Text;

namespace XiletradeAuth.Models;

internal sealed class PkceGenerator
{
    public string CodeVerifier { get; }
    public string CodeChallenge { get; }

    /// <summary>
    /// verifierLength (between 32 and 96 bytes) according to RFC 7636 specifications
    /// </summary>
    /// <param name="verifierLength"></param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    internal PkceGenerator(int verifierLength = 32)
    {
        if (verifierLength < 32 || verifierLength > 96)
            throw new ArgumentOutOfRangeException(nameof(verifierLength)
                , "Verifier length must be between 32 and 96 bytes.");

        var verifierBytes = new byte[verifierLength];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(verifierBytes);
        }

        CodeVerifier = Base64UrlEncode(verifierBytes);
        var hash = SHA256.HashData(Encoding.ASCII.GetBytes(CodeVerifier));
        CodeChallenge = Base64UrlEncode(hash);
    }

    // Method to encode URL-safe base64 without padding
    private static string Base64UrlEncode(byte[] input)
    {
        return Convert.ToBase64String(input)
            .Replace("+", "-")
            .Replace("/", "_")
            .TrimEnd('=');
    }

    internal void Display()
    {
        Console.WriteLine($"Code Verifier: {CodeVerifier}");
        Console.WriteLine($"Code Challenge: {CodeChallenge}");
    }
}
