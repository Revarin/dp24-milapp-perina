using Kris.Client.Utility;
using Kris.Client.ViewModels;

namespace Kris.Client
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            var vm = ServiceHelper.GetService<AppShellViewModel>();
            if (vm == null) throw new ArgumentNullException(nameof(vm));
            MainPage = new AppShell(vm);
        }
    }
}
