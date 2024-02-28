using Kris.Client.Data.Cache;
using Kris.Client.Utility;
using Microsoft.Maui.Maps;
using System.ComponentModel;

using MauiMap = Microsoft.Maui.Controls.Maps.Map;

namespace Kris.Client.Behaviors.Map;

public sealed class CurrentRegionBehavior : BindableBehavior<MauiMap>
{
    private const string VisibleRegionProperty = "VisibleRegion";

    private readonly ILocationStore _locationStore;

    public static readonly BindableProperty ValueProperty = BindableProperty.Create(
        "Value", typeof(MapSpan), typeof(CurrentRegionBehavior), default(MapSpan), BindingMode.OneWayToSource);
    public static readonly BindableProperty CacheProperty = BindableProperty.Create(
        "Cache", typeof(double), typeof(CurrentRegionBehavior), 1.0, BindingMode.OneWayToSource);

    public MapSpan Value
    {
        get { return (MapSpan)GetValue(ValueProperty); }
        private set { SetValue(ValueProperty, value); }
    }
    public double Cache
    {
        get { return (double)GetValue(CacheProperty); }
        private set { SetValue(CacheProperty, value); }
    }

    private DateTime _nextSave;

    public CurrentRegionBehavior() : base()
    {
        _locationStore = ServiceHelper.GetService<ILocationStore>();
        _nextSave = DateTime.MinValue;
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
        if (e.PropertyName == VisibleRegionProperty)
        {
            Value = AssociatedObject.VisibleRegion;

            if (Cache > 0 && DateTime.Now > _nextSave)
            {
                _locationStore.StoreCurrentRegion(Value);
                _nextSave = _nextSave.AddSeconds(Cache);
            }
        }
    }
}
