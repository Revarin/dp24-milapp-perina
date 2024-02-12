using Kris.Client.ViewModels;

namespace Kris.Client;

public partial class TestView : ContentPage
{
	public TestView(TestViewModel viewModel)
	{
		BindingContext = viewModel;
		InitializeComponent();
	}
}