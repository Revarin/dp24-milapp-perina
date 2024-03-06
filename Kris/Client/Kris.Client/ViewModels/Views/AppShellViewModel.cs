﻿using CommunityToolkit.Mvvm.ComponentModel;
using Kris.Client.Views;

namespace Kris.Client.ViewModels.Views;

public sealed class AppShellViewModel : ObservableObject
{
    public AppShellViewModel()
    {
        Routing.RegisterRoute(nameof(RegisterView), typeof(RegisterView));
        Routing.RegisterRoute(nameof(SessionSettingsView), typeof(SessionSettingsView));
        Routing.RegisterRoute(nameof(UserSettingsView), typeof(UserSettingsView));
        Routing.RegisterRoute(nameof(ContactsView), typeof(ContactsView));
    }
}
