namespace Kris.Client.Core.Models;

public sealed class AvailableSessionsModel
{
    public SessionListModel CurrentSession { get; set; }
    public IEnumerable<SessionListModel> JoinedSessions { get; set; }
    public IEnumerable<SessionListModel> OtherSessions { get; set; }
}
