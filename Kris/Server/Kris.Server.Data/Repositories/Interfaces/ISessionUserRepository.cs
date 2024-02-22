using Kris.Common.Enums;
using Kris.Server.Data.Models;

namespace Kris.Server.Data.Repositories;

public interface ISessionUserRepository : IRepository<SessionUserEntity>
{
    Task<SessionUserEntity?> GetAsync(Guid userId, Guid sessionId, CancellationToken ct);
    Task<bool> AuthorizeAsync(Guid userId, Guid sessionId, UserType minRole, CancellationToken ct);
}
