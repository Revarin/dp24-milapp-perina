using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;

using MauiMap = Microsoft.Maui.Controls.Maps.Map;

namespace Kris.Client.Behaviors
{
    public class MapRegionBehavior : BindableBehavior<MauiMap>
    {
        public static readonly BindableProperty CurrentRegionProperty = BindableProperty.Create(
            "CurrentRegion", typeof(MapSpan), typeof(MapRegionBehavior), default(MapSpan), BindingMode.TwoWay,
            propertyChanged: OnCurrentRegionChanged);

        public MapSpan CurrentRegion
        {
            get { return (MapSpan)GetValue(CurrentRegionProperty); }
            set { SetValue(CurrentRegionProperty, value); }
        }

        private static void OnCurrentRegionChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var thisInstance = (bindable as MapRegionBehavior)?.AssociatedObject;
            var newRegion = newValue as MapSpan;

            thisInstance?.MoveToRegion(newRegion);
        }
    }
}
