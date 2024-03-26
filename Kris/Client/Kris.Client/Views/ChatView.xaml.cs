using Kris.Client.ViewModels.Views;

namespace Kris.Client.Views;

public partial class ChatView : ContentPage
{
	public ChatView(ChatViewModel vm)
	{
		BindingContext = vm;
		InitializeComponent();
	}
}