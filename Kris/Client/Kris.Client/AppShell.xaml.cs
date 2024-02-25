using Kris.Client.ViewModels.Views;
using Kris.Client.Views;

namespace Kris.Client
{
    public partial class AppShell : Shell
    {
        public AppShell(AppShellViewModel vm)
        {
            BindingContext = vm;
            InitializeComponent();
        }
    }
}
