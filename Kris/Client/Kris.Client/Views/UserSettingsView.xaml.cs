using Kris.Client.ViewModels.Views;

namespace Kris.Client.Views;

public partial class UserSettingsView : ContentPage
{
	public UserSettingsView(UserSettingsViewModel vm)
	{
		BindingContext = vm;
		InitializeComponent();
	}
}