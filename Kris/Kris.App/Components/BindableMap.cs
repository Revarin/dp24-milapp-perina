using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace Kris.App.Components
{
    // Source: https://stackoverflow.com/a/53804163
    public class BindableMap : Xamarin.Forms.Maps.Map
    {
        public static readonly BindableProperty PositionProperty = BindableProperty.Create("MapSpan", typeof(MapSpan), typeof(BindableMap),
            null, BindingMode.TwoWay, propertyChanged: PositionPropertyChanged);

        public MapSpan Position
        {
            get { return (MapSpan)GetValue(PositionProperty); }
            set { SetValue(PositionProperty, value); }
        }

        public BindableMap()
        {
        }

        private static void PositionPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var thisInstance = bindable as BindableMap;
            var newMapSpan = newValue as MapSpan;

            thisInstance?.MoveToRegion(newMapSpan);
        }
    }
}
