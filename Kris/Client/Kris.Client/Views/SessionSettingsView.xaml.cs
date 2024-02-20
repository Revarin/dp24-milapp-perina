using Kris.Client.ViewModels;

namespace Kris.Client.Views;

public partial class SessionSettingsView : ContentPage
{
	public SessionSettingsView(SessionSettingsViewModel vm)
	{
		BindingContext = vm;
		InitializeComponent();
		(BindingContext as SessionSettingsViewModel).Init();
	}
}