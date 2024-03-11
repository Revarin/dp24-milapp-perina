using Kris.Client.Components.Map;
using Kris.Client.ViewModels.Views;

namespace Kris.Client.Views;

public partial class MapView : ContentPage
{
	public MapView(MapViewModel vm)
	{
		BindingContext = vm;
		InitializeComponent();
		(BindingContext as MapViewModel).Init();
	}

    private async void KrisMapPin_MarkerClicked(object sender, Microsoft.Maui.Controls.Maps.PinClickedEventArgs e)
    {
		if (sender is KrisMapPin krisPin) await (BindingContext as MapViewModel).OnKrisPinClicked(krisPin, e);
    }
}