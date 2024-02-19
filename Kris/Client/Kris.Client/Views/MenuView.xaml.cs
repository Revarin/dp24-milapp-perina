using Kris.Client.ViewModels;

namespace Kris.Client.Views;

public partial class MenuView : ContentPage
{
	public MenuView(MenuViewModel vm)
	{
		BindingContext = vm;
		InitializeComponent();
		(BindingContext as MenuViewModel).Init();
	}
}