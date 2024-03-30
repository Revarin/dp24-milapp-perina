using Kris.Client.Common.Enums;

namespace Kris.Client.ViewModels.Items;

public sealed class KrisMapPinViewModel
{
    public Guid Id { get; init; }
    public Guid CreatorId { get; init; }
    public string CreatorName { get; set; }
    public string Name { get; set; }
    public Location Location { get; set; }
    public DateTime TimeStamp { get; set; }
    public KrisPinType KrisPinType { get; set; }
    public string ImageName { get; set; }
}
