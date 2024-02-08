using Kris.Server.Data.Models;
using Microsoft.AspNetCore.Identity;

namespace Kris.Server.Core.Services;

public sealed class UserPasswordService : IPasswordService<UserEntity>
{
    private readonly IPasswordHasher<UserEntity> _passwordHasher;

    public UserPasswordService(IPasswordHasher<UserEntity> passwordHasher)
    {
        _passwordHasher = passwordHasher;
    }

    public string HashPassword(UserEntity user, string password)
    {
        return _passwordHasher.HashPassword(user, password);
    }

    public bool VerifyPassword(UserEntity user, string password)
    {
        if (user.Password == null) return false;

        var result = _passwordHasher.VerifyHashedPassword(user, user.Password, password);
        return result == PasswordVerificationResult.Success;
    }
}
