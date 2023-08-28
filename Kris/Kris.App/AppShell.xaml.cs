using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Kris.App
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
        }
    }
}