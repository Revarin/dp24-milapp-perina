using Kris.Client.ViewModels.Views;

namespace Kris.Client.Views;

public partial class ContactsView : ContentPage
{
	public ContactsView(ContactsViewModel vm)
	{
		BindingContext = vm;
		InitializeComponent();
    }
}