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

public sealed partial class MapSettingsViewModel : PageViewModelBase
{
    private readonly IMapSettingsDataProvider _mapSettingsDataProvider;

    [ObservableProperty]
    private ObservableCollection<CoordinateSystemItem> _coordinateSystemItems;
    [ObservableProperty]
    private CoordinateSystemItem _coordinateSystemSelectedItem;

    public MapSettingsViewModel(IMapSettingsDataProvider mapSettingsDataProvider,
        IMediator mediator, IRouterService navigationService, IMessageService messageService, IAlertService alertService)
        : base(mediator, navigationService, messageService, alertService)
    {
        _mapSettingsDataProvider = mapSettingsDataProvider;

        _coordinateSystemItems = _mapSettingsDataProvider.GetCoordinateSystemItems().ToObservableCollection();
        _coordinateSystemSelectedItem = _mapSettingsDataProvider.GetCurrentCoordinateSystem();
    }

    // HANDLERS
    [RelayCommand]
    private async Task OnMapSettingsSelectedIndexChanged() => await UpdateMapSettingsAsync();

    // CORE
    private async Task UpdateMapSettingsAsync()
    {
        var ct = new CancellationToken();
        var command = new UpdateMapSettingsCommand
        {
            MapSettings = new MapSettingsEntity
            {
                CoordinateSystem = CoordinateSystemSelectedItem.Value
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
            _messageService.Send(new MapSettingsChangedMessage());
        }
    }
}
