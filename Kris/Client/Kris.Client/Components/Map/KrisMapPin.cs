using Kris.Client.Common.Enums;
using Microsoft.Maui.Controls.Maps;

namespace Kris.Client.Components.Map;

public sealed class KrisMapPin : Pin, IKrisMapPin
{
    public static readonly BindableProperty KrisTypeProperty = BindableProperty.Create(
    "KrisType", typeof(KrisPinType), typeof(KrisMapPin));

    public static readonly BindableProperty ImageSourceProperty = BindableProperty.Create(
    "ImageSource", typeof(ImageSource), typeof(KrisMapPin));

    public KrisPinType KrisType
    {
        get { return (KrisPinType)GetValue(KrisTypeProperty); }
        set { SetValue(KrisTypeProperty, value); }
    }
    public ImageSource ImageSource
    {
        get { return (ImageSource)GetValue(ImageSourceProperty); }
        set { SetValue(ImageSourceProperty, value); }
    }
}
