using Kris.Client.ViewModels;

namespace Kris.Client;

public partial class ConnectionSettingsView : ContentPage
{
	public ConnectionSettingsView(ConnectionSettingsViewModel viewModel)
	{
		BindingContext = viewModel;
		InitializeComponent();
	}
}