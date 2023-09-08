using MauiMap = Microsoft.Maui.Controls.Maps.Map;

namespace Kris.Client.Behaviors
{
    public class MoveToMapRegionBehavior : BindableBehavior<MauiMap>
    {
        public static readonly BindableProperty RequestProperty = BindableProperty.Create(
            "Request", typeof(MoveToRegionRequest), typeof(MoveToMapRegionBehavior), default(MoveToRegionRequest), BindingMode.OneWay,
            propertyChanged: OnMoveToRegionChanged);

        public MoveToRegionRequest Request
        {
            get { return (MoveToRegionRequest)GetValue(RequestProperty); }
            set { SetValue(RequestProperty, value); }
        }

        private static void OnMoveToRegionChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as MoveToMapRegionBehavior)?.OnMoveToRegionChanged(oldValue as MoveToRegionRequest, newValue as MoveToRegionRequest);
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
