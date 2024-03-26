using Kris.Client.ViewModels.Views;

namespace Kris.Client.Views;

public partial class MenuView : ContentPage
{
	public MenuView(MenuViewModel vm)
	{
		BindingContext = vm;
		InitializeComponent();
	}
}