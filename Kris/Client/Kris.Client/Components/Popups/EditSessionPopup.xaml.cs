using CommunityToolkit.Maui.Views;
using Kris.Client.ViewModels.Popups;

namespace Kris.Client.Components.Popups;

public partial class EditSessionPopup : Popup
{
	public EditSessionPopup(EditSessionPopupViewModel vm)
	{
		BindingContext = vm;
		InitializeComponent();
        (BindingContext as EditSessionPopupViewModel).CancelClosing += CancelClose;
        (BindingContext as EditSessionPopupViewModel).LoadErrorClosing += ReturnClose;
        (BindingContext as EditSessionPopupViewModel).UpdatedClosing += ReturnClose;
        (BindingContext as EditSessionPopupViewModel).DeletedClosing += ReturnClose;
        (BindingContext as EditSessionPopupViewModel).Init();
    }

    private async void CancelClose(object sender, EventArgs e) => await CloseAsync();

    private async void ReturnClose(object sender, EventArgs e) => await CloseAsync(e);
}