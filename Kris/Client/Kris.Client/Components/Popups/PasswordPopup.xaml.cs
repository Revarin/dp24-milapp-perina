using CommunityToolkit.Maui.Views;
using Kris.Client.Components.Events;
using Kris.Client.ViewModels.Popups;

namespace Kris.Client.Components.Popups;

public partial class PasswordPopup : Popup
{
	public PasswordPopup(PasswordPopupViewModel vm)
	{
		BindingContext = vm;
		InitializeComponent();
        (BindingContext as PasswordPopupViewModel).CancelClosing += OnCancelClosing;
        (BindingContext as PasswordPopupViewModel).AcceptedClosing += OnAcceptClosing;
        (BindingContext as PasswordPopupViewModel).Init();
    }

    private async void OnCancelClosing(object sender, EventArgs e) => await CloseAsync();

    private async void OnAcceptClosing(object sender, PasswordEventArgs e) => await CloseAsync(e);
}