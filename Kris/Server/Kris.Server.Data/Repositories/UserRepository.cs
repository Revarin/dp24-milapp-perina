using Kris.Server.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Kris.Server.Data.Repositories;

public sealed class UserRepository : RepositoryBase<UserEntity>, IUserRepository
{
    public UserRepository(DataContext dataContext) : base(dataContext)
    {
    }

    public async Task<UserEntity?> GetByLoginAsync(string login, CancellationToken ct)
    {
        return await _context.Users
            .Include(user => user.Settings)
            .Include(user => user.AllSessions)
            .Include(user => user.CurrentSession).ThenInclude(sessionUser => sessionUser == null ? null : sessionUser.Session)
            .FirstOrDefaultAsync(user => user.Login == login);
    }


    public async Task<UserEntity?> GetWithSessionsAsync(Guid id, CancellationToken ct)
    {
        return await _context.Users
            .Include(user => user.AllSessions)
            .Include(user => user.CurrentSession)
            .ThenInclude(sessionUser => sessionUser == null ? null : sessionUser.Session)
            .FirstOrDefaultAsync(user => user.Id == id);
    }

    public async Task<UserEntity?> GetWithSettingsAsync(Guid id, CancellationToken ct)
    {
        return await _context.Users
            .Include(user => user.Settings)
            .FirstOrDefaultAsync(user => user.Id == id);
    }

    public async Task<bool> UserExistsAsync(string login, CancellationToken ct)
    {
        return await _context.Users.AnyAsync(user => user.Login == login, ct);
    }
}
