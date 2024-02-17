using Kris.Client.ViewModels;

namespace Kris.Client.Views;

public partial class LoginView : ContentPage
{
	public LoginView(LoginViewModel vm)
	{
		BindingContext = vm;
		InitializeComponent();
	}
}