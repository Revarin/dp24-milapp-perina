using Kris.Client.ViewModels.Views;

namespace Kris.Client.Views;

public partial class LoginView : ContentPage
{
	public LoginView(LoginViewModel vm)
	{
		BindingContext = vm;
		InitializeComponent();
		(BindingContext as LoginViewModel).Init();
	}
}