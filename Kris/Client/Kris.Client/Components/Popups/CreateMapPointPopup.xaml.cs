using CommunityToolkit.Maui.Views;
using Kris.Client.ViewModels.Popups;

namespace Kris.Client.Components.Popups;

public partial class CreateMapPointPopup : Popup
{
	public CreateMapPointPopup(CreateMapPointPopupViewModel vm)
	{
		BindingContext = vm;
		InitializeComponent();
		(BindingContext as CreateMapPointPopupViewModel).CancelClosing += CancelClose;
		(BindingContext as CreateMapPointPopupViewModel).CreatedClosing += ReturnClose;
		(BindingContext as CreateMapPointPopupViewModel).Init();
	}

    private async void CancelClose(object sender, EventArgs e) => await CloseAsync();

	private async void ReturnClose(object sender, EventArgs e) => await CloseAsync(e);
}