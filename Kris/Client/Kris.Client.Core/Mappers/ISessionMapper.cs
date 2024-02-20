using Kris.Client.Core.Models;
using Kris.Interface.Models;

namespace Kris.Client.Core.Mappers;

public interface ISessionMapper
{
    SessionListModel Map(SessionModel model);
}
