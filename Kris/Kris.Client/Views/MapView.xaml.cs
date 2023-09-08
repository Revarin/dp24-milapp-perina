using Kris.Client.ViewModels;

namespace Kris.Client;

public partial class MapView : ContentPage
{
	public MapView(MapViewModel viewModel)
	{
		BindingContext = viewModel;
		InitializeComponent();
	}
}