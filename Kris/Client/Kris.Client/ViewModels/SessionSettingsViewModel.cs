using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Kris.Client.Core.Models;
using Kris.Client.Core.Services;
using MediatR;
using System.Collections.ObjectModel;

namespace Kris.Client.ViewModels;

public sealed partial class SessionSettingsViewModel : ViewModelBase
{
    [ObservableProperty]
    private ObservableCollection<SessionListModel> _sessions;

    public SessionSettingsViewModel(IMediator mediator, IRouterService navigationService, IAlertService alertService)
        : base(mediator, navigationService, alertService)
    {
    }

    protected override Task InitAsync()
    {
        // Get all sessions
        throw new NotImplementedException();
    }

    [RelayCommand]
    private Task OnCreateSessionClicked()
    {
        throw new NotImplementedException();
    }
}
