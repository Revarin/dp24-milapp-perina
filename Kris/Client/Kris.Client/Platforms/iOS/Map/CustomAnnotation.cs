using MapKit;
using Microsoft.Maui.Maps;
using UIKit;

namespace Kris.Client.Platforms.Map;

// Source: https://vladislavantonyuk.github.io/articles/Customize-map-pins-in-.NET-MAUI/
public class CustomAnnotation : MKPointAnnotation
{
    public Guid Identifier { get; init; }
    public UIImage Image { get; init; }
    public required IMapPin Pin { get; init; }
}