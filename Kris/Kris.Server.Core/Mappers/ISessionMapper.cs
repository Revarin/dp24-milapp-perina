using Kris.Interface.Models;
using Kris.Server.Core.Requests;
using Kris.Server.Data.Models;

namespace Kris.Server.Core.Mappers;

public interface ISessionMapper
{
    SessionEntity Map(CreateSessionCommand command);
    SessionModel Map(SessionEntity entity);
}
