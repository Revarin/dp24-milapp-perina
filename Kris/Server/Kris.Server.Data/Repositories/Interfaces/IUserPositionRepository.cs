﻿using Kris.Server.Data.Models;

namespace Kris.Server.Data.Repositories;

public interface IUserPositionRepository : IRepository<UserPositionEntity>
{
    Task<IEnumerable<UserPositionEntity>> GetWithUsersAsync(Guid sessionId, CancellationToken ct);
    Task<IEnumerable<UserPositionEntity>> GetWithUsersAsync(Guid sessionId, DateTime from, CancellationToken ct);
}
