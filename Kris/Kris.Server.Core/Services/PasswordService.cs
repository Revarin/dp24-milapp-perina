namespace Kris.Server.Core.Services;

public sealed class PasswordService : IPasswordService
{
    public string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.EnhancedHashPassword(password);
    }

    public bool VerifyPassword(string hash, string password)
    {
        if (string.IsNullOrEmpty(hash)) return false;
        if (string.IsNullOrEmpty(password)) return false;

        return BCrypt.Net.BCrypt.EnhancedVerify(password, hash);
    }
}
