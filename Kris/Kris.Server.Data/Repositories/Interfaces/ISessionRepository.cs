﻿using Kris.Server.Data.Models;

namespace Kris.Server.Data.Repositories;

public interface ISessionRepository : IRepository<SessionEntity>
{
    Task<bool> SessionExistsAsync(string name, CancellationToken ct);
}