using CommunityToolkit.Maui.Views;
using Kris.Client.ViewModels.Popups;

namespace Kris.Client.Components.Popups;

public partial class EditMapPointPopup : Popup
{
	public EditMapPointPopup(EditMapPointPopupViewModel vm)
	{
		BindingContext = vm;
		InitializeComponent();
		(BindingContext as EditMapPointPopupViewModel).CancelClosing += CancelClose;
		(BindingContext as EditMapPointPopupViewModel).LoadErrorClosing += ReturnClose;
		(BindingContext as EditMapPointPopupViewModel).DeletedClosing += ReturnClose;
		(BindingContext as EditMapPointPopupViewModel).UpdatedClosing += ReturnClose;
        (BindingContext as EditMapPointPopupViewModel).Init();
	}

    private async void CancelClose(object sender, EventArgs e) => await CloseAsync();

	private async void ReturnClose(object sender, EventArgs e) => await CloseAsync(e);
}