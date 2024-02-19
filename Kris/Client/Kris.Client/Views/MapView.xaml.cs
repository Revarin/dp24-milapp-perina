using Kris.Client.ViewModels;

namespace Kris.Client.Views;

public partial class MapView : ContentPage
{
	public MapView(MapViewModel vm)
	{
		BindingContext = vm;
		InitializeComponent();
		(BindingContext as MapViewModel).Init();
	}
}