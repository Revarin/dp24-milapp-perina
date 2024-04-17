using Kris.Client.Components.Events;
using Kris.Client.Utility;
using Microsoft.Maui.Maps;
using System.Runtime.CompilerServices;

using MauiMap = Microsoft.Maui.Controls.Maps.Map;

namespace Kris.Client.Components.Map;

public sealed class KrisMap : MauiMap, IKrisMap
{
    public event EventHandler<MapLongClickedEventArgs> MapLongClicked;
    public event EventHandler<CurrentRegionChangedEventArgs> CurrentRegionChanged;
    public event EventHandler<EventArgs> CameraMoveStarted;

    public static readonly BindableProperty CurrentRegionProperty = BindableProperty.Create(
        "CurrentRegion", typeof(MapSpan), typeof(KrisMap), default(MapSpan), BindingMode.OneWayToSource);

    public static readonly BindableProperty MoveToRegionRequestProperty = BindableProperty.Create(
        "MoveToRegionRequest", typeof(MoveToRegionRequest), typeof(KrisMap), default(MoveToRegionRequest), BindingMode.OneWay,
        propertyChanged: OnMoveToRegionChanged);

    public static readonly BindableProperty KrisMapStyleProperty = BindableProperty.Create(
        "KrisMapStyle", typeof(KrisMapStyle), typeof(KrisMap), null);

    public MapSpan CurrentRegion
    {
        get { return (MapSpan)GetValue(CurrentRegionProperty); }
        private set { SetValue(CurrentRegionProperty, value); }
    }
    public MoveToRegionRequest MoveToRegionRequest
    {
        get { return (MoveToRegionRequest)GetValue(MoveToRegionRequestProperty); }
        set { SetValue(MoveToRegionRequestProperty, value); }
    }
    public KrisMapStyle KrisMapStyle
    {
        get { return (KrisMapStyle)GetValue(KrisMapStyleProperty); }
        set { SetValue(KrisMapStyleProperty, value); }
    }

    public KrisMap()
    {
    }

    public void LongClicked(Location location)
    {
        MapLongClicked?.Invoke(this, new MapLongClickedEventArgs(location));
    }

    public void MoveStarted()
    {
        CameraMoveStarted?.Invoke(this, EventArgs.Empty);
    }

    protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        if (propertyName == nameof(VisibleRegion))
        {
            CurrentRegion = VisibleRegion;
            CurrentRegionChanged?.Invoke(this, new CurrentRegionChangedEventArgs(CurrentRegion));
        }

        base.OnPropertyChanged(propertyName);
    }

    private static void OnMoveToRegionChanged(BindableObject bindable, object oldValue, object newValue)
    {
        (bindable as KrisMap)?.OnMoveToRegionChanged(oldValue as MoveToRegionRequest, newValue as MoveToRegionRequest);
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

    private void OnMoveToRegionRequested(object sender, MoveToRegionEventArgs e)
    {
        if (e.Region != null)
        {
            MoveToRegion(e.Region);
        }
    }
}
