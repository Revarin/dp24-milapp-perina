namespace Kris.Client.Core.Models;

public sealed class SessionListModel
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public DateTime Created { get; set; }
    public int UserCount { get; set; }
}
