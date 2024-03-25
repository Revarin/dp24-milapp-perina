using CommunityToolkit.Maui.Views;
using Kris.Client.ViewModels.Popups;

namespace Kris.Client.Components.Popups;

public partial class LoadingPopup : Popup
{
	public LoadingPopup(LoadingPopupViewModel vm)
	{
		BindingContext = vm;
		InitializeComponent();
		(BindingContext as LoadingPopupViewModel).FinishClosing += CancelClose;
    }

    private async void CancelClose(object sender, EventArgs e) => await CloseAsync();
}