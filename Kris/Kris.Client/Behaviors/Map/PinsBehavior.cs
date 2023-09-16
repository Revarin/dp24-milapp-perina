using System.Collections.ObjectModel;
using Microsoft.Maui.Controls.Maps;

using MauiMap = Microsoft.Maui.Controls.Maps.Map;

namespace Kris.Client.Behaviors
{
    public class PinsBehavior : BindableBehavior<MauiMap>
    {
        private static readonly BindablePropertyKey PinsSourcePropertyKey = BindableProperty.CreateReadOnly(
            "PinsSource", typeof(ObservableCollection<Pin>), typeof(PinsBehavior), default(ObservableCollection<Pin>));

        public static readonly BindableProperty PinsSourceProperty = PinsSourcePropertyKey.BindableProperty;

        public ObservableCollection<Pin> PinsSource
        {
            get => (ObservableCollection<Pin>)GetValue(PinsSourceProperty);
            private set => SetValue(PinsSourcePropertyKey, value);
        }

        protected override void OnAttachedTo(MauiMap bindable)
        {
            base.OnAttachedTo(bindable);
            PinsSource = bindable.Pins as ObservableCollection<Pin>;
        }

        protected override void OnDetachingFrom(MauiMap bindable)
        {
            base.OnDetachingFrom(bindable);
            PinsSource = null;
        }
    }
}
