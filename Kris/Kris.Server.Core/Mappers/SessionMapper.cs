using Kris.Common.Enums;
using Kris.Server.Core.Requests;
using Kris.Server.Core.Services;
using Kris.Server.Data.Models;

namespace Kris.Server.Core.Mappers;

public sealed class SessionMapper : ISessionMapper
{
    private readonly IPasswordService<SessionEntity> _passwordService;

    public SessionMapper(IPasswordService<SessionEntity> passwordService)
    {
        _passwordService = passwordService;
    }

    public SessionEntity Map(CreateSessionCommand command)
    {
        var session = new SessionEntity
        {
            Id = Guid.NewGuid(),
            Name = command.CreateSession.Name,
            IsActive = true,
            Created = DateTime.UtcNow,
        };

        session.Password = _passwordService.HashPassword(session, command.CreateSession.Password);
        session.Users.Add(new SessionUserEntity
        {
            SessionId = session.Id,
            UserId = command.User.Id,
            UserType = UserType.SuperAdmin,
            Joined = DateTime.UtcNow
        });

        return session;
    }
}
