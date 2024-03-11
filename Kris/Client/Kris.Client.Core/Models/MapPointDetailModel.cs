using Kris.Common.Models;

namespace Kris.Client.Core.Models;

public sealed class MapPointDetailModel
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public DateTime Created { get; set; }
    public UserListModel Creator { get; set; }
    public Location Location { get; set; }
    public MapPointSymbol Symbol { get; set; }
    public string Description { get; set; }
}
