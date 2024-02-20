using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Kris.Client.Core.Models;
using Kris.Client.Core.Requests;
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

    protected override async Task InitAsync()
    {
        var ct = new CancellationToken();
        var query = new GetSessionsQuery();
        var result = await _mediator.Send(query, ct);

        if (result.IsFailed)
        {
            // TODO
        }
        else
        {
            Sessions = result.Value.ToObservableCollection();
        }
    }

    [RelayCommand]
    private Task OnCreateSessionClicked()
    {
        throw new NotImplementedException();
    }
}
