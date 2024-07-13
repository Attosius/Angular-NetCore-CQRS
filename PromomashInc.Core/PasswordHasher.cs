using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;
using PromomashInc.Core.Models;

namespace PromomashInc.Core
{
    public class CustomPasswordHasher : ICustomPasswordHasher
    {
        public string GetHash(string password)
        {
            var salt = RandomNumberGenerator.GetBytes(128 / 8);
            var hash = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8));
            return hash;
        }
    }
}
