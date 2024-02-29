namespace Kris.Client.Core.Models;

public sealed class UserPositionModel
{
    public Guid UserId { get; set; }
    public string? UserName { get; set; }
    public DateTime Updated { get; set; }
    public List<Location> Positions { get; set; }
}
