using Kris.Common.Enums;
using Kris.Interface.Models;
using Kris.Server.Core.Requests;
using Kris.Server.Core.Services;
using Kris.Server.Data.Models;

namespace Kris.Server.Core.Mappers;

public sealed class SessionMapper : ISessionMapper
{
    private readonly IPasswordService _passwordService;

    public SessionMapper(IPasswordService passwordService)
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

        session.Password = _passwordService.HashPassword(command.CreateSession.Password);
        session.Users.Add(new SessionUserEntity
        {
            SessionId = session.Id,
            UserId = command.User.Id,
            UserType = UserType.SuperAdmin,
            Joined = DateTime.UtcNow
        });

        return session;
    }

    public SessionModel Map(SessionEntity entity)
    {
        return new SessionModel
        {
            Id = entity.Id,
            Name = entity.Name,
            Created = entity.Created,
            IsActive = entity.IsActive,
            UserCount = entity.Users.Count()
        };
    }
}
