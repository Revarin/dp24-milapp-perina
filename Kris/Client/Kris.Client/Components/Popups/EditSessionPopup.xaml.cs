using CommunityToolkit.Maui.Views;
using Kris.Client.ViewModels.Popups;

namespace Kris.Client.Components.Popups;

public partial class EditSessionPopup : Popup
{
	public EditSessionPopup(EditSessionPopupViewModel vm)
	{
		BindingContext = vm;
		InitializeComponent();
        (BindingContext as EditSessionPopupViewModel).CancelClosing += CancelClosing;
        (BindingContext as EditSessionPopupViewModel).LoadErrorClosing += ReturnClosing;
        (BindingContext as EditSessionPopupViewModel).UpdatedClosing += ReturnClosing;
        (BindingContext as EditSessionPopupViewModel).DeletedClosing += ReturnClosing;
        (BindingContext as EditSessionPopupViewModel).Init();
    }

    private async void CancelClosing(object sender, EventArgs e) => await CloseAsync();

    private async void ReturnClosing(object sender, EventArgs e) => await CloseAsync(e);
}