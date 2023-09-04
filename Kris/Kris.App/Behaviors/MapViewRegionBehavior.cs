using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Kris.App.Common;

using GoogleMap = Xamarin.Forms.Maps.Map;

namespace Kris.App
{
    public class MapViewRegionBehavior : BehaviorBase<GoogleMap>
    {
        public static readonly BindableProperty CurrentRegionProperty = BindableProperty.Create(
            "CurrentRegion", typeof(MapSpan), typeof(MapViewRegionBehavior), default(MapSpan), BindingMode.OneWayToSource);
        public static readonly BindableProperty MoveToRegionProperty = BindableProperty.Create(
            "MoveToRegion", typeof(MoveToRegionRequest), typeof(MapViewRegionBehavior), default(MoveToRegionRequest), BindingMode.OneWay,
            propertyChanged: OnMoveToRegionChanged);

        public MapSpan CurrentRegion
        {
            get => (MapSpan)GetValue(CurrentRegionProperty);
            set => SetValue(CurrentRegionProperty, value);
        }
        public MoveToRegionRequest MoveToRegion
        {
            get => (MoveToRegionRequest)GetValue(MoveToRegionProperty);
            set => SetValue(MoveToRegionProperty, value);
        }

        protected override void OnAttachedTo(GoogleMap bindable)
        {
            base.OnAttachedTo(bindable);
            bindable.PropertyChanged += OnCurrentRegionChanged;
        }

        protected override void OnDetachingFrom(GoogleMap bindable)
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
            (bindable as MapViewRegionBehavior)?.OnMoveToRegionChanged(oldValue as MoveToRegionRequest, newValue as MoveToRegionRequest);
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

        private void OnMoveToRegionRequested(object sender, MoveToRegionRequestEventArgs moveToRegionRequestedEventArgs)
        {
            if (moveToRegionRequestedEventArgs.MapSpan != null)
            {
                AssociatedObject.MoveToRegion(moveToRegionRequestedEventArgs.MapSpan);
            }
        }
    }
}
