using Kris.Client.Common.Enums;

namespace Kris.Client.ViewModels.Views;

public sealed class KrisMapPinViewModel
{
    public Guid Id { get; init; }
    public Guid CreatorId { get; init; }
    public string Name { get; set; }
    public string Description { get; set; }
    public Location Location { get; set; }
    public DateTime TimeStamp { get; set; }
    public KrisPinType KrisPinType { get; set; }
    public ImageSource ImageSource { get; set; }
    public string ImageName { get; set; }
    public Func<View> View { get; set; }
}
