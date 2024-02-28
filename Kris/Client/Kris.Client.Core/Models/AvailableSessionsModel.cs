using Kris.Common.Enums;

namespace Kris.Client.Core.Models;

public sealed class AvailableSessionsModel
{
    public UserType? UserType { get; set; }
    public SessionListModel? CurrentSession { get; set; }
    public IEnumerable<SessionListModel> JoinedSessions { get; set; }
    public IEnumerable<SessionListModel> OtherSessions { get; set; }
}
