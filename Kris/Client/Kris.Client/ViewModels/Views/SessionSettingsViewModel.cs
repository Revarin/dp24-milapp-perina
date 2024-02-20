using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Kris.Client.Common.Errors;
using Kris.Client.Common.Utility;
using Kris.Client.Core.Models;
using Kris.Client.Core.Requests;
using Kris.Client.Core.Services;
using Kris.Client.ViewModels.Popups;
using Kris.Client.Views;
using Kris.Common.Extensions;
using MediatR;
using System.Collections.ObjectModel;

namespace Kris.Client.ViewModels.Views;

public sealed partial class SessionSettingsViewModel : ViewModelBase
{
    private readonly IPopupService _popupService;

    [ObservableProperty]
    private SessionListModel _currentSession;
    [ObservableProperty]
    private ObservableCollection<SessionListModel> _joinedSessions;
    [ObservableProperty]
    private ObservableCollection<SessionListModel> _otherSessions;

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
            if (result.HasError<UnauthorizedError>())
            {
                await _alertService.ShowToastAsync("Login expired");
                await _mediator.Send(new LogoutUserCommand(), ct);
                await _navigationService.GoToAsync(nameof(LoginView), RouterNavigationType.ReplaceUpward);
            }
            else
            {
                await _alertService.ShowToastAsync(result.Errors.FirstMessage());
            }
        }
        else
        {
            CurrentSession = result.Value.CurrentSession;
            JoinedSessions = result.Value.JoinedSessions.ToObservableCollection();
            OtherSessions = result.Value.OtherSessions.ToObservableCollection();
        }
    }

    [RelayCommand]
    private async Task OnCreateSessionClicked()
    {
        await _popupService.ShowPopupAsync<EditSessionPopupViewModel>();
    }
}
