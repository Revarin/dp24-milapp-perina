using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Kris.Client.Common.Errors;
using Kris.Client.Components.Events;
using Kris.Client.Core.Messages;
using Kris.Client.Core.Requests;
using Kris.Client.Core.Services;
using Kris.Client.Data.Models;
using Kris.Client.Data.Models.Picker;
using Kris.Client.Data.Providers;
using Kris.Client.Validations;
using Kris.Client.ViewModels.Popups;
using Kris.Common.Extensions;
using MediatR;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace Kris.Client.ViewModels.Views;

public sealed partial class UserSettingsViewModel : PageViewModelBase
{
    private readonly IPopupService _popupService;
    private readonly IConnectionSettingsDataProvider _settingsDataProvider;

    // Connection settings
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
    [ObservableProperty]
    private ObservableCollection<MapObjectDownloadSettingsItem> _mapObjectDownloadItems;
    [ObservableProperty]
    private MapObjectDownloadSettingsItem _mapObjectDownloadSelectedItem;

    // User edit
    [Required]
    [ObservableProperty]
    private string _login;
    [Required]
    [ObservableProperty]
    private string _password;
    [Required]
    [Match("Password", "Passwords do not match")]
    [ObservableProperty]
    private string _passwordVerification;

    public UserSettingsViewModel(IPopupService popupService, IConnectionSettingsDataProvider settingsDataProvider,
        IMediator mediator, IRouterService navigationService, IMessageService messageService, IAlertService alertService)
        : base(mediator, navigationService, messageService, alertService)
    {
        _popupService = popupService;
        _settingsDataProvider = settingsDataProvider;

        _gpsIntervalItems = _settingsDataProvider.GetGpsIntervalSettingsItems().ToObservableCollection();
        _gpsIntervalSelectedItem = _settingsDataProvider.GetCurrentGpsIntervalSettings();
        _positionUploadItems = _settingsDataProvider.GetPositionUploadSettingsItems().ToObservableCollection();
        _positionUploadSelectedItem = _settingsDataProvider.GetCurrentPositionUploadSettings();
        _positionDownloadItems = _settingsDataProvider.GetPositionDownloadSettingsItems().ToObservableCollection();
        _positionDownloadSelectedItem = _settingsDataProvider.GetCurrentPositionDownloadSettings();
        _mapObjectDownloadItems = _settingsDataProvider.GetMapObjectDownloadSettingsItems().ToObservableCollection();
        _mapObjectDownloadSelectedItem = _settingsDataProvider.GetCurrentMapObjectDownloadSettings();
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
                PositionDownloadInterval = PositionDownloadSelectedItem.Value,
                MapObjectDownloadInterval = MapObjectDownloadSelectedItem.Value
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

    [RelayCommand]
    private async Task OnEditUserClicked()
    {
        if (ValidateAllProperties()) return;

        var passwordPopup = await _popupService.ShowPopupAsync<PasswordPopupViewModel>() as PasswordEventArgs;
        if (passwordPopup == null) return;

        var ct = new CancellationToken();
        var command = new EditUserCommand { NewLogin = Login, NewPassword = Password, Password = passwordPopup.Password };
        var result = await _mediator.Send(command, ct);

        if (result.IsFailed)
        {
            if (result.HasError<UnauthorizedError>())
            {
                await _alertService.ShowToastAsync("Login expired");
                await LogoutUser();
            }
            else if (result.HasError<ForbiddenError>())
            {
                await _alertService.ShowToastAsync("Wrong password");
            }
            else
            {
                await _alertService.ShowToastAsync(result.Errors.FirstMessage());
            }
        }
        else
        {
            Cleanup();
            await _alertService.ShowToastAsync("User edited");
        }
    }

    protected override void Cleanup()
    {
        Login = string.Empty;
        Password = string.Empty;
        PasswordVerification = string.Empty;

        base.Cleanup();
    }
}
