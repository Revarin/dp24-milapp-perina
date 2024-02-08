using Kris.Server.Data.Models;

namespace Kris.Server.Core.Services;

public interface IPasswordService<TEntity> where TEntity : EntityBase
{
    string HashPassword(TEntity user, string password);
    bool VerifyPassword(TEntity user, string password);
}
