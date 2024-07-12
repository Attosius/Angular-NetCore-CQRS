using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;

namespace PromomashInc.Core
{
    public class CustomPasswordHasher : ICustomPasswordHasher
    {
        public string GetHash(string password)
        {
            byte[] salt = RandomNumberGenerator.GetBytes(128 / 8); // divide by 8 to convert bits to bytes
            var hash = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8));
            return hash;
        }
    }

    public interface ICustomPasswordHasher
    {
        string GetHash(string password);
    }
}
