using CommunityToolkit.Maui.Views;
using Kris.Client.ViewModels.Utility;

namespace Kris.Client.Components.Utility;

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