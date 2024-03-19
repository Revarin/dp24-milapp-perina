using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Kris.Client.Common.Errors;
using Kris.Client.Core.Messages;
using Kris.Client.Core.Requests;
using Kris.Client.Core.Services;
using Kris.Client.Data.Cache;
using Kris.Client.Data.Database;
using Kris.Client.Data.Models;
using Kris.Client.Data.Models.Picker;
using Kris.Client.Data.Providers;
using Kris.Common.Enums;
using Kris.Common.Extensions;
using MediatR;
using System.Collections.ObjectModel;

namespace Kris.Client.ViewModels.Views;

public sealed partial class MapSettingsViewModel : PageViewModelBase
{
    private readonly IMapSettingsDataProvider _mapSettingsDataProvider;
    private readonly IMediaService _mediaService;
    private readonly IFileStore _fileStore;
    private readonly IRepositoryFactory _repositoryFactory;

    [ObservableProperty]
    private ObservableCollection<CoordinateSystemItem> _coordinateSystemItems;
    [ObservableProperty]
    private CoordinateSystemItem _coordinateSystemSelectedItem;
    [ObservableProperty]
    private ObservableCollection<MapTypeItem> _mapTypeItems;
    [ObservableProperty]
    private MapTypeItem _mapTypeSelectedItem;
    [ObservableProperty]
    private string _customMapTileSourcePath;

    public MapSettingsViewModel(IMapSettingsDataProvider mapSettingsDataProvider, IMediaService mediaService,
        IFileStore fileStore, IRepositoryFactory repositoryFactory,
        IMediator mediator, IRouterService navigationService, IMessageService messageService, IAlertService alertService)
        : base(mediator, navigationService, messageService, alertService)
    {
        _mapSettingsDataProvider = mapSettingsDataProvider;
        _mediaService = mediaService;
        _fileStore = fileStore;
        _repositoryFactory = repositoryFactory;

        CoordinateSystemItems = _mapSettingsDataProvider.GetCoordinateSystemItems().ToObservableCollection();
        CoordinateSystemSelectedItem = _mapSettingsDataProvider.GetCurrentCoordinateSystem();
        MapTypeItems = _mapSettingsDataProvider.GetMapTypeItems().ToObservableCollection();
        MapTypeSelectedItem = _mapSettingsDataProvider.GetCurrentMapType();
        CustomMapTileSourcePath = _mapSettingsDataProvider.GetCurrentCustomMapTileSource();
    }

    // HANDLERS
    [RelayCommand]
    private async Task OnCoordinateSystemSelectedIndexChanged() => await UpdateCoordinateSystemSettingsAsync();
    [RelayCommand]
    private async Task OnMapTypeSelectedIndexChanged() => await UpdateMapTypeSettingsAsync();

    // CORE
    private async Task UpdateCoordinateSystemSettingsAsync()
    {
        await UpdateMapSettingsAsync();
    }

    private async Task UpdateMapTypeSettingsAsync()
    {
        CustomMapTileSourcePath = null;

        if (MapTypeSelectedItem.Value == KrisMapType.Custom)
        {
            CustomMapTileSourcePath = await PickMapTileDatabase();
            if (string.IsNullOrEmpty(CustomMapTileSourcePath))
            {
                await _alertService.ShowToastAsync("Pick a valid map tile database");
                MapTypeSelectedItem = MapTypeItems.First();
            }
        }

        await UpdateMapSettingsAsync();
    }

    private async Task UpdateMapSettingsAsync()
    {
        var ct = new CancellationToken();
        var command = new UpdateMapSettingsCommand
        {
            MapSettings = new MapSettingsEntity
            {
                CoordinateSystem = CoordinateSystemSelectedItem.Value,
                MapType = MapTypeSelectedItem.Value
            },
            CustomMapTilesDatabasePath = CustomMapTileSourcePath
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

    private async Task<string> PickMapTileDatabase()
    {
        // TODO: Two DB copy: shared file -> cache -> appdata
        // Direct copy or shared file access require platform code
        var fileResult = await _mediaService.PickMapTileDatabaseAsync();
        if (fileResult == null) return string.Empty;

        using var tilesRepository = _repositoryFactory.CreateMapTileRepository(fileResult.FullPath);
        var dbValid = tilesRepository.IsDbSchemaValid();
        if (!dbValid) return string.Empty;

        var path = _fileStore.SaveToData(fileResult.FileName, await fileResult.OpenReadAsync());
        return path;
    }
}
