using System;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Scuttle.Application.Common.Interfaces;

namespace Scuttle.Infrastructure.Authentication;

public class PasswordHasher : IPasswordHasher
{
    private const int SaltSize = 128 / 8; // 16 bytes
    private const int KeySize = 256 / 8; // 32 bytes
    private const int Iterations = 1000;
    private static readonly KeyDerivationPrf Prf = KeyDerivationPrf.HMACSHA256;
    private const char Delimiter = ':';

    public string HashPassword(string password)
    {
        if (string.IsNullOrEmpty(password))
            throw new ArgumentException("Password cannot be null or empty", nameof(password));

        // generate salt
        byte[] salt = new byte[SaltSize];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(salt);
        }

        // Derive a key from the password
        byte[] key = KeyDerivation.Pbkdf2(
            password: password,
            salt: salt,
            prf: Prf,
            iterationCount: Iterations,
            numBytesRequested: KeySize
        );

        // Format as a base64 with delimitation
        return string.Join(
            Delimiter,
            Convert.ToBase64String(salt),
            Convert.ToBase64String(key)
        );
    }

    public bool VerifyPassword(string password, string hash)
    {
        if (string.IsNullOrEmpty(password) || string.IsNullOrEmpty(hash))
            return false;

        // Split the hash into salt and key
        string[] parts = password.Split(Delimiter);
        if (parts.Length != 2)
            return false;

        // Extract the salt and hash from the stored val
        byte[] salt;
        byte[] key;

        try
        {
            salt = Convert.FromBase64String(parts[0]);
            key = Convert.FromBase64String(parts[1]);
        }
        catch (FormatException)
        {
            return false;
        }

        // Derive the key using the same params
        byte[] derivedKey = KeyDerivation.Pbkdf2(
            password: password,
            salt: salt,
            prf: Prf,
            iterationCount: Iterations,
            numBytesRequested: KeySize
        );

        // Time-constant comparison to prevent timing attacks
        return CryptographicOperations.FixedTimeEquals(key, derivedKey);
    }
}
