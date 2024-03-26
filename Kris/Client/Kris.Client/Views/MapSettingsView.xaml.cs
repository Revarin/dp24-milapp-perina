using Kris.Client.ViewModels.Views;

namespace Kris.Client.Views;

public partial class MapSettingsView : ContentPage
{
	public MapSettingsView(MapSettingsViewModel vm)
	{
		BindingContext = vm;
		InitializeComponent();
	}
}