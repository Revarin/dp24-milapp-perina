using CommunityToolkit.Maui.Views;
using Kris.Client.Events;
using Kris.Client.ViewModels.Popups;

namespace Kris.Client.Components.Popups;

public partial class EditSessionPopup : Popup
{
	public EditSessionPopup(EditSessionPopupViewModel vm)
	{
		BindingContext = vm;
		InitializeComponent();
        (BindingContext as EditSessionPopupViewModel).CancelClosing += OnCancelClosing;
		(BindingContext as EditSessionPopupViewModel).CreatedClosing += OnCreatedClosing;
        (BindingContext as EditSessionPopupViewModel).Init();
	}

    private async void OnCancelClosing(object sender, EventArgs e) => await CloseAsync();

	private async void OnCreatedClosing(object sender, ResultEventArgs e) => await CloseAsync(e); 
}