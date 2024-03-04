namespace Kris.Client.Core.Models;

public class MapObjectModel
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime Created { get; set; }
    public UserListModel Creator { get; set; }
}
