using Kris.Client.ViewModels.Views;

namespace Kris.Client.Views;

public partial class RegisterView : ContentPage
{
	public RegisterView(RegisterViewModel vm)
	{
		BindingContext = vm;
		InitializeComponent();
		(BindingContext as RegisterViewModel).Init();
	}
}