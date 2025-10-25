using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace TestFunction
{
    internal class PasswordHash
    {
        public static string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }

        public static bool VerifyPassword(string password, string passwordHash)
        {
            var hash = HashPassword(password);
            return hash == passwordHash;
        }
        public static void Main()
        {
            var password = "123456";
            var hashedPassword = HashPassword(password);
            Console.WriteLine($"Hashed Password: {hashedPassword}");
            var isMatch = VerifyPassword("my_secure_password", hashedPassword);
            Console.WriteLine($"Password Match: {isMatch}");
        }
    }
}
