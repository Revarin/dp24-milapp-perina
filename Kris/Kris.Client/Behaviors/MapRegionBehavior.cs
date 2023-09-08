using System.ComponentModel;
using Microsoft.Maui.Maps;

using MauiMap = Microsoft.Maui.Controls.Maps.Map;

namespace Kris.Client.Behaviors
{
    // Source: https://github.com/nuitsjp/Xamarin.Forms.GoogleMaps.Bindings/tree/master
    // Source: https://stackoverflow.com/questions/28098020/bind-to-xamarin-forms-maps-map-from-viewmodel/53804163#53804163
    // TODO:
    // Split to CurrentMapRegionBehavior and MoveToMapRegionBehavior
    // Add SaveToPreferences property and StoringInterval property
    public class MapRegionBehavior : BindableBehavior<MauiMap>
    {
        public static readonly BindableProperty CurrentRegionProperty = BindableProperty.Create(
            "CurrentRegion", typeof(MapSpan), typeof(MapRegionBehavior), default(MapSpan), BindingMode.OneWayToSource);
        public static readonly BindableProperty MoveToRegionProperty = BindableProperty.Create(
            "MoveToRegion", typeof(MoveToRegionRequest), typeof(MapRegionBehavior), default(MoveToRegionRequest), BindingMode.OneWay,
            propertyChanged: OnMoveToRegionChanged);

        public MapSpan CurrentRegion
        {
            get { return (MapSpan)GetValue(CurrentRegionProperty); }
            private set { SetValue(CurrentRegionProperty, value); }
        }
        public MoveToRegionRequest MoveToRegion
        {
            get { return (MoveToRegionRequest)GetValue(MoveToRegionProperty); }
            set { SetValue(MoveToRegionProperty, value); }
        }

        protected override void OnAttachedTo(MauiMap bindable)
        {
            base.OnAttachedTo(bindable);
            bindable.PropertyChanged += OnCurrentRegionChanged;
        }

        protected override void OnDetachingFrom(MauiMap bindable)
        {
            bindable.PropertyChanged -= OnCurrentRegionChanged;
            base.OnDetachingFrom(bindable);
        }

        private void OnCurrentRegionChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "VisibleRegion")
            {
                CurrentRegion = AssociatedObject.VisibleRegion;
            }
        }

        private static void OnMoveToRegionChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as MapRegionBehavior)?.OnMoveToRegionChanged(oldValue as MoveToRegionRequest, newValue as MoveToRegionRequest);
        }

        private void OnMoveToRegionChanged(MoveToRegionRequest oldValue, MoveToRegionRequest newValue)
        {
            if (oldValue != null)
            {
                oldValue.MoveToRegionRequested -= OnMoveToRegionRequested;
            }
            if (newValue != null)
            {
                newValue.MoveToRegionRequested += OnMoveToRegionRequested;
            }
        }

        private void OnMoveToRegionRequested(object sender, MoveToRegionRequestEventArgs e)
        {
            if (e.MapSpan != null)
            {
                AssociatedObject.MoveToRegion(e.MapSpan);
            }
        }
    }
}
