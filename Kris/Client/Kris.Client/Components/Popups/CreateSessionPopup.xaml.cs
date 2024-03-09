using CommunityToolkit.Maui.Views;
using Kris.Client.ViewModels.Popups;

namespace Kris.Client.Components.Popups;

public partial class CreateSessionPopup : Popup
{
	public CreateSessionPopup(CreateSessionPopupViewModel vm)
	{
		BindingContext = vm;
		InitializeComponent();
        (BindingContext as CreateSessionPopupViewModel).CancelClosing += CancelClose;
		(BindingContext as CreateSessionPopupViewModel).CreatedClosing += ReturnClose;
        (BindingContext as CreateSessionPopupViewModel).Init();
	}

    private async void CancelClose(object sender, EventArgs e) => await CloseAsync();

	private async void ReturnClose(object sender, EventArgs e) => await CloseAsync(e); 
}