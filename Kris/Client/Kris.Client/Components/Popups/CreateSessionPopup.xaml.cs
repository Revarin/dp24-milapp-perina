using CommunityToolkit.Maui.Views;
using Kris.Client.ViewModels.Popups;

namespace Kris.Client.Components.Popups;

public partial class CreateSessionPopup : Popup
{
	public CreateSessionPopup(CreateSessionPopupViewModel vm)
	{
		BindingContext = vm;
		InitializeComponent();
        (BindingContext as CreateSessionPopupViewModel).CancelClosing += CancelClosing;
		(BindingContext as CreateSessionPopupViewModel).CreatedClosing += ReturnClosing;
        (BindingContext as CreateSessionPopupViewModel).Init();
	}

    private async void CancelClosing(object sender, EventArgs e) => await CloseAsync();

	private async void ReturnClosing(object sender, EventArgs e) => await CloseAsync(e); 
}