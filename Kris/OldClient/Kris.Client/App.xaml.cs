using Kris.Client.ViewModels;

namespace Kris.Client
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            MainPage = new AppShell(ServiceHelper.GetService<AppShellViewModel>());
        }
    }
}