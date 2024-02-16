using Kris.Client.ViewModels;

namespace Kris.Client;

public partial class MenuView : ContentPage
{
	public MenuView(MenuViewModel viewModel)
	{
		BindingContext = viewModel;
		InitializeComponent();
	}
}