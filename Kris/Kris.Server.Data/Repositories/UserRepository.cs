using Kris.Server.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Kris.Server.Data.Repositories;

public sealed class UserRepository : RepositoryBase<UserEntity>, IUserRepository
{
    public UserRepository(DataContext dataContext) : base(dataContext)
    {
    }

    public async Task<bool> UserExistsAsync(string login, CancellationToken ct)
    {
        return await _context.Users.AnyAsync(user => user.Login == login, ct);
    }
}
