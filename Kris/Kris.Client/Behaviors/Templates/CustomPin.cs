using Microsoft.Maui.Controls.Maps;

namespace Kris.Client.Behaviors
{
    public class CustomPin : Pin
    {
        public static readonly BindableProperty ImageSourceProperty = BindableProperty.Create(
            "ImageSource", typeof(ImageSource), typeof(CustomPin));

        public ImageSource ImageSource
        {
            get { return (ImageSource)GetValue(ImageSourceProperty); }
            set { SetValue(ImageSourceProperty, value); }
        }
    }
}
