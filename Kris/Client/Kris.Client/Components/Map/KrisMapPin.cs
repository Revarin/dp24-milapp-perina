using Kris.Client.Common.Enums;
using Microsoft.Maui.Controls.Maps;

namespace Kris.Client.Components.Map;

public sealed class KrisMapPin : Pin, IKrisMapPin
{
    public static readonly BindableProperty KrisIdProperty = BindableProperty.Create(
        "KrisId", typeof(Guid), typeof(KrisMapPin));

    public static readonly BindableProperty KrisTypeProperty = BindableProperty.Create(
        "KrisType", typeof(KrisPinType), typeof(KrisMapPin));

    public static readonly BindableProperty ImageNameProperty = BindableProperty.Create(
        "ImageName", typeof(string), typeof(KrisMapPin));


    public Guid KrisId
    {
        get { return (Guid)GetValue(KrisIdProperty); }
        set { SetValue(KrisIdProperty, value); }
    }

    public KrisPinType KrisType
    {
        get { return (KrisPinType)GetValue(KrisTypeProperty); }
        set { SetValue(KrisTypeProperty, value); }
    }

    public string ImageName
    {
        get { return (string)GetValue(ImageNameProperty); }
        set { SetValue(ImageNameProperty, value); }
    }
}
