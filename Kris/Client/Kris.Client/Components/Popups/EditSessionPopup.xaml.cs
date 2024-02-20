using CommunityToolkit.Maui.Views;
using Kris.Client.ViewModels.Popups;

namespace Kris.Client.Components.Popups;

public partial class EditSessionPopup : Popup
{
	public EditSessionPopup(EditSessionPopupViewModel vm)
	{
		BindingContext = vm;
		InitializeComponent();
        (BindingContext as EditSessionPopupViewModel).RaiseClosePopupEvent += OnClose;
        (BindingContext as EditSessionPopupViewModel).Init();
	}

    private void OnClose(object sender, EventArgs e) => Close();
}