using Kris.Interface.Models;
using Kris.Server.Data.Models;

namespace Kris.Server.Core.Mappers;

public interface ISessionMapper
{
    SessionListModel MapList(SessionEntity entity);
    SessionDetailModel MapDetail(SessionEntity entity, Guid userId);
}
