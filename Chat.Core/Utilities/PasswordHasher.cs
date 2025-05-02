using System;
using System.Security.Cryptography;

namespace Chat.Core.Utilities
{
    public static class PasswordHasher
    {
        public static (string Hash, string Salt) CreateHash(string password)
        {
            var salt = GenerateSalt();
            var hash = ComputeHash(password, salt);
            return (hash, salt);
        }

        public static bool VerifyHash(string password, string hash, string salt)
        {
            var computedHash = ComputeHash(password, salt);
            return hash == computedHash;
        }

        private static string GenerateSalt()
        {
            var saltBytes = new byte[16];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(saltBytes);
            }
            return Convert.ToBase64String(saltBytes);
        }

        private static string ComputeHash(string password, string salt)
        {
            using (var sha256 = SHA256.Create())
            {
                var saltedPassword = password + salt;
                var hashBytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(saltedPassword));
                return Convert.ToBase64String(hashBytes);
            }
        }
    }

}
