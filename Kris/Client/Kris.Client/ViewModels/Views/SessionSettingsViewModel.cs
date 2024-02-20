using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Kris.Client.Core.Models;
using Kris.Client.Core.Requests;
using Kris.Client.Core.Services;
using Kris.Client.ViewModels.Popups;
using MediatR;
using System.Collections.ObjectModel;

namespace Kris.Client.ViewModels.Views;

public sealed partial class SessionSettingsViewModel : ViewModelBase
{
    private readonly IPopupService _popupService;

    [ObservableProperty]
    private ObservableCollection<SessionListModel> _sessions;

    public SessionSettingsViewModel(IPopupService popupService,
        IMediator mediator, IRouterService navigationService, IAlertService alertService)
        : base(mediator, navigationService, alertService)
    {
        _popupService = popupService;
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
    private async Task OnCreateSessionClicked()
    {
        await _popupService.ShowPopupAsync<EditSessionPopupViewModel>();
    }
}
