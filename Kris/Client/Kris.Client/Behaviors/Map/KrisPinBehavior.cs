using Microsoft.Maui.Controls.Maps;

namespace Kris.Client.Behaviors.Map;

public sealed class KrisPinBehavior : Pin
{
    public static readonly BindableProperty ImageSourceProperty = BindableProperty.Create(
        "ImageSource", typeof(ImageSource), typeof(KrisPinBehavior));

    public ImageSource ImageSource
    {
        get { return (ImageSource)GetValue(ImageSourceProperty); }
        set { SetValue(ImageSourceProperty, value); }
    }
}
