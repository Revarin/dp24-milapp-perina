using CommunityToolkit.Maui.Views;
using Kris.Client.Components.Events;
using Kris.Client.ViewModels.Utility;

namespace Kris.Client.Components.Utility;

public partial class PasswordPopup : Popup
{
	public PasswordPopup(PasswordPopupViewModel vm)
	{
		BindingContext = vm;
		InitializeComponent();
        (BindingContext as PasswordPopupViewModel).CancelClosing += CancelClose;
        (BindingContext as PasswordPopupViewModel).AcceptedClosing += ReturnClose;
    }

    private async void CancelClose(object sender, EventArgs e) => await CloseAsync();

    private async void ReturnClose(object sender, PasswordEventArgs e) => await CloseAsync(e);
}
