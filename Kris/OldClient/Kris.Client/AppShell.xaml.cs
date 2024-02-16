using Kris.Client.ViewModels;

namespace Kris.Client
{
    public partial class AppShell : Shell
    {
        public AppShell(AppShellViewModel viewModel)
        {
            BindingContext = viewModel;
            InitializeComponent();
        }
    }
}