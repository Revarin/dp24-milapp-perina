using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Kris.Client.Common.Errors;
using Kris.Client.Core.Messages;
using Kris.Client.Core.Requests;
using Kris.Client.Core.Services;
using Kris.Client.Data.Models;
using Kris.Client.Data.Models.Picker;
using Kris.Client.Data.Providers;
using Kris.Common.Extensions;
using MediatR;
using System.Collections.ObjectModel;

namespace Kris.Client.ViewModels.Views;

public sealed partial class UserSettingsViewModel : PageViewModelBase
{
    private readonly IConnectionSettingsDataProvider _settingsDataProvider;

    [ObservableProperty]
    private ObservableCollection<GpsIntervalSettingsItem> _gpsIntervalItems;
    [ObservableProperty]
    private GpsIntervalSettingsItem _gpsIntervalSelectedItem;
    [ObservableProperty]
    private ObservableCollection<PositionUploadSettingsItem> _positionUploadItems;
    [ObservableProperty]
    private PositionUploadSettingsItem _positionUploadSelectedItem;
    [ObservableProperty]
    private ObservableCollection<PositionDownloadSettingsItem> _positionDownloadItems;
    [ObservableProperty]
    private PositionDownloadSettingsItem _positionDownloadSelectedItem;

    public UserSettingsViewModel(IConnectionSettingsDataProvider settingsDataProvider,
        IMediator mediator, IRouterService navigationService, IMessageService messageService, IAlertService alertService)
        : base(mediator, navigationService, messageService, alertService)
    {
        _settingsDataProvider = settingsDataProvider;

        _gpsIntervalItems = _settingsDataProvider.GetGpsIntervalSettingsItems().ToObservableCollection();
        _gpsIntervalSelectedItem = _settingsDataProvider.GetCurrentGpsIntervalSettings();
        _positionUploadItems = _settingsDataProvider.GetPositionUploadSettingsItems().ToObservableCollection();
        _positionUploadSelectedItem = _settingsDataProvider.GetCurrentPositionUploadSettings();
        _positionDownloadItems = _settingsDataProvider.GetPositionDownloadSettingsItems().ToObservableCollection();
        _positionDownloadSelectedItem = _settingsDataProvider.GetCurrentPositionDownloadSettings();
    }

    [RelayCommand]
    private async Task OnConnectionSettingsSelectedIndexChanged()
    {
        var ct = new CancellationToken();
        var command = new UpdateConnectionSettingsCommand
        {
            ConnectionSettings = new ConnectionSettingsEntity
            {
                GpsInterval = GpsIntervalSelectedItem.Value,
                PositionUploadMultiplier = PositionUploadSelectedItem.Value,
                PositionDownloadInterval = PositionDownloadSelectedItem.Value
            }
        };
        var result = await _mediator.Send(command, ct);

        if (result.IsFailed)
        {
            if (result.HasError<UnauthorizedError>())
            {
                await _alertService.ShowToastAsync("Login expired");
                await LogoutUser();
            }
            else
            {
                await _alertService.ShowToastAsync(result.Errors.FirstMessage());
            }
        }
        else
        {
            _messageService.Send(new ConnectionSettingsChangedMessage());
        }
    }
}
