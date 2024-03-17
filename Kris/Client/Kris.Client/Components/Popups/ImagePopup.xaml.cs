using CommunityToolkit.Maui.Views;
using Kris.Client.ViewModels.Popups;

namespace Kris.Client.Components.Popups;

public partial class ImagePopup : Popup
{
	public ImagePopup(ImagePopupViewModel vm)
	{
		BindingContext = vm;
		InitializeComponent();
        (BindingContext as ImagePopupViewModel).CancelClosing += CancelClose;
    }

    private async void CancelClose(object sender, EventArgs e) => await CloseAsync();
}