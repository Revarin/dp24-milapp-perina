using Kris.Server.Data.Models;

namespace Kris.Server.Core.Services;

public interface IPasswordService
{
    string HashPassword(string password);
    bool VerifyPassword(string hash, string password);
}
