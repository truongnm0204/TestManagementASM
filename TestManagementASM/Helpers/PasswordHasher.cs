using System.Security.Cryptography;
using System.Text;

namespace TestManagementASM.Helpers;

public static class PasswordHasher
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
