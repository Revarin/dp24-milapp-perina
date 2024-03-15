using CommunityToolkit.Mvvm.ComponentModel;
using Kris.Client.Views;

namespace Kris.Client.ViewModels.Views;

public sealed class AppShellViewModel : ObservableObject
{
    public AppShellViewModel()
    {
        Routing.RegisterRoute(nameof(RegisterView), typeof(RegisterView));
        Routing.RegisterRoute(nameof(SessionSettingsView), typeof(SessionSettingsView));
        Routing.RegisterRoute(nameof(UserSettingsView), typeof(UserSettingsView));
        Routing.RegisterRoute(nameof(MapSettingsView), typeof(MapSettingsView));
        Routing.RegisterRoute(nameof(ContactsView), typeof(ContactsView));
        Routing.RegisterRoute(nameof(ChatView), typeof(ChatView));
    }
}
