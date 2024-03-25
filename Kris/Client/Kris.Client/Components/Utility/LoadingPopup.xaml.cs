using CommunityToolkit.Maui.Views;
using Kris.Client.ViewModels.Utility;

namespace Kris.Client.Components.Utility;

public partial class LoadingPopup : Popup
{
	public LoadingPopup(LoadingPopupViewModel vm)
	{
		BindingContext = vm;
		InitializeComponent();
		(BindingContext as LoadingPopupViewModel).CancelClosing += CancelClose;
    }

    private async void CancelClose(object sender, EventArgs e) => await CloseAsync();
}