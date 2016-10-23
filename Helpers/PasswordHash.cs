using System;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace NoteCloud.Helpers {
    public class PasswordHash {
        public static bool verify(string password, string passwordhash, string salt) {
            return (passwordhash == PasswordHash.hash(password, salt));
        }

        public static Tuple<string, string> hash(string password) {
            string salt = PasswordHash.createSalt();
            return Tuple.Create(hash(password, salt), salt);
        }

        public static string hash(string password, string salt) {
            // derive a 256-bit subkey (use HMACSHA1 with 10,000 iterations)
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: Convert.FromBase64String(salt),
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));

            return hashed;
        }

        public static string createSalt() {
            // generate a 128-bit salt using a secure PRNG
            byte[] salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            return Convert.ToBase64String(salt);
        }
    }
}
