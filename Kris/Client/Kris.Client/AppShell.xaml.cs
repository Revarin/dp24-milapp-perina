using Kris.Client.ViewModels.Views;

namespace Kris.Client
{
    public partial class AppShell : Shell
    {
        public AppShell(AppShellViewModel vm)
        {
            BindingContext = vm;
            InitializeComponent();
            (BindingContext as AppShellViewModel).Init();
        }
    }
}
