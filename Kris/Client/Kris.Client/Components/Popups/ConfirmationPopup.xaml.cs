using CommunityToolkit.Maui.Views;
using Kris.Client.ViewModels.Popups;

namespace Kris.Client.Components.Popups;

public partial class ConfirmationPopup : Popup
{
	public ConfirmationPopup(ConfirmationPopupViewModel vm)
	{
		BindingContext = vm;
		InitializeComponent();
		(BindingContext as ConfirmationPopupViewModel).YesClosing += ReturnClose;
		(BindingContext as ConfirmationPopupViewModel).NoClosing += ReturnClose;
    }

    private async void ReturnClose(object sender, EventArgs e) => await CloseAsync(e);
}