using Kris.Server.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Kris.Server.Data.Repositories;

public sealed class UserRepository : RepositoryBase<UserEntity>, IUserRepository
{
    public UserRepository(DataContext dataContext) : base(dataContext)
    {
    }

    public async Task<UserEntity?> GetWithCurrentSessionAsync(Guid id, CancellationToken ct)
    {
        return await _context.Users.Include(user => user.CurrentSession)
            .ThenInclude(sessionUser => sessionUser == null ? null : sessionUser.Session)
            .FirstOrDefaultAsync(user => user.Id == id);
    }

    public async Task<UserEntity?> GetWithAllSessionsAsync(Guid id, CancellationToken ct)
    {
        return await _context.Users.Include(user => user.AllSessions)
            .Include(user => user.CurrentSession)
            .FirstOrDefaultAsync(user => user.Id == id);
    }

    public async Task<UserEntity?> GetWithCurrentSessionAsync(string login, CancellationToken ct)
    {
        return await _context.Users.Include(user => user.CurrentSession)
            .ThenInclude(sessionUser => sessionUser == null ? null : sessionUser.Session)
            .FirstOrDefaultAsync(user => user.Login == login);
    }

    public async Task<bool> UserExistsAsync(string login, CancellationToken ct)
    {
        return await _context.Users.AnyAsync(user => user.Login == login, ct);
    }
}
