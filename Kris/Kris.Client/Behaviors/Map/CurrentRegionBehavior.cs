using System.ComponentModel;
using Microsoft.Maui.Maps;
using Kris.Client.Core;
using Kris.Client.Common;

using MauiMap = Microsoft.Maui.Controls.Maps.Map;

namespace Kris.Client.Behaviors
{
    // Source: https://github.com/nuitsjp/Xamarin.Forms.GoogleMaps.Bindings/tree/master
    // Source: https://stackoverflow.com/questions/28098020/bind-to-xamarin-forms-maps-map-from-viewmodel/53804163#53804163
    public class CurrentRegionBehavior : BindableBehavior<MauiMap>
    {
        private readonly IPreferencesStore _preferencesStore;

        public static readonly BindableProperty ValueProperty = BindableProperty.Create(
            "Value", typeof(MapSpan), typeof(CurrentRegionBehavior), default(MapSpan), BindingMode.OneWayToSource);
        public static readonly BindableProperty CacheProperty = BindableProperty.Create(
            "Cache", typeof(bool), typeof(CurrentRegionBehavior), false, BindingMode.OneWayToSource);
        public static readonly BindableProperty CachePeriodProperty = BindableProperty.Create(
            "CachePeriod", typeof(int), typeof(CurrentRegionBehavior), 1000, BindingMode.OneWayToSource);

        public MapSpan Value
        {
            get { return (MapSpan)GetValue(ValueProperty); }
            private set { SetValue(ValueProperty, value); }
        }
        public bool Cache
        {
            get { return (bool)GetValue(CacheProperty); }
            private set { SetValue(CacheProperty, value); }
        }
        public int CachePeriod
        {
            get { return (int)GetValue(CachePeriodProperty); }
            private set { SetValue(CachePeriodProperty, value); }
        }

        private DateTime _nextSave;

        public CurrentRegionBehavior() : base()
        {
            _preferencesStore = ServiceHelper.GetService<IPreferencesStore>();
            _nextSave = DateTime.Now;
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
                Value = AssociatedObject.VisibleRegion;
                if (Cache && DateTime.Now > _nextSave)
                {
                    _preferencesStore.Set(Constants.PreferencesStore.LastRegionKey, Value);
                    _nextSave = _nextSave.AddMilliseconds(CachePeriod);
                }
            }
        }
    }
}
