using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FluentResults;
using Kris.Client.Common.Events;
using Kris.Client.Converters;
using Kris.Client.Core.Models;
using Kris.Client.Core.Requests;
using Kris.Client.Data.Models.Picker;
using Kris.Client.Data.Providers;
using Kris.Client.Utility;
using MediatR;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace Kris.Client.ViewModels.Popups;

public sealed partial class CreateMapPointPopupViewModel : PopupViewModel
{
    private readonly IMapSettingsDataProvider _mapSettingsDataProvider;
    private readonly IMapPointSymbolDataProvider _symbolDataProvider;
    private readonly ISymbolImageComposer _symbolImageComposer;

    public Guid CurrentUserId { get; set; }
    public string CurrentUserName { get; set; }

    [Required]
    [ObservableProperty]
    private string _pointName;
    [ObservableProperty]
    private string _description;
    [ObservableProperty]
    private LocationCoordinates _locationCoordinates;

    [ObservableProperty]
    private ObservableCollection<MapPointSymbolColorItem> _mapPointColorItems;
    [Required]
    [ObservableProperty]
    private MapPointSymbolColorItem _mapPointColorSelectedItem;
    [ObservableProperty]
    private ObservableCollection<MapPointSymbolShapeItem> _mapPointShapeItems;
    [Required]
    [ObservableProperty]
    private MapPointSymbolShapeItem _mapPointShapeSelectedItem;
    [ObservableProperty]
    private ObservableCollection<MapPointSymbolSignItem> _mapPointSignItems;
    [Required]
    [ObservableProperty]
    private MapPointSymbolSignItem _mapPointSignSelectedItem;

    [ObservableProperty]
    private ImageSource _image;

    public event EventHandler<ResultEventArgs<MapPointListModel>> CreatedClosing;

    public CreateMapPointPopupViewModel(IMapSettingsDataProvider mapSettingsDataProvider, IMapPointSymbolDataProvider symbolDataProvider,
        ISymbolImageComposer symbolImageComposer, IMediator mediator)
        : base(mediator)
    {
        _mapSettingsDataProvider = mapSettingsDataProvider;
        _symbolDataProvider = symbolDataProvider;
        _symbolImageComposer = symbolImageComposer;

        MapPointColorItems = _symbolDataProvider.GetMapPointSymbolColorItems().ToObservableCollection();
        MapPointShapeItems = _symbolDataProvider.GetMapPointSymbolShapeItems().ToObservableCollection();
        MapPointSignItems = _symbolDataProvider.GetMapPointSymbolSignItems().ToObservableCollection();
    }

    // HANDLERS
    [RelayCommand]
    private void OnSymbolComponentChanged() => RedrawSymbol();
    [RelayCommand]
    private async Task OnCreateButtonClicked() => await CreateMapPointAsync();

    // CORE
    public void Setup(Guid userId, string userName, Location location)
    {
        CurrentUserId = userId;
        CurrentUserName = userName;
        LocationCoordinates = new LocationCoordinates
        {
            Location = location,
            CoordinateSystem = _mapSettingsDataProvider.GetCurrentCoordinateSystem().Value
        };
    }

    private void RedrawSymbol()
    {
        var pointShape = MapPointShapeSelectedItem?.Value;
        var pointColor = MapPointColorSelectedItem?.Value;
        var pointSign = MapPointSignSelectedItem?.Value;

        var imageStream = _symbolImageComposer.ComposeMapPointSymbol(pointShape, pointColor, pointSign);
        Image = ImageSource.FromStream(() => imageStream);
    }

    private async Task CreateMapPointAsync()
    {
        if (ValidateAllProperties())
        {
            if (ErrorMessages.ContainsKey(nameof(MapPointColorSelectedItem))
                || ErrorMessages.ContainsKey(nameof(MapPointShapeSelectedItem))
                || ErrorMessages.ContainsKey(nameof(MapPointSignSelectedItem)))
            {
                AddCustomError("MapSymbol", "Map symbol must be selected");
            }
            return;
        }

        var ct = new CancellationToken();
        var command = new CreateMapPointCommand
        {
            Name = PointName,
            Description = Description,
            Location = LocationCoordinates.Location,
            Shape = MapPointShapeSelectedItem.Value,
            Color = MapPointColorSelectedItem.Value,
            Sign = MapPointSignSelectedItem.Value
        };
        var result = await _mediator.Send(command, ct);

        var returnResult = result.IsSuccess
            ? Result.Ok(new MapPointListModel
            {
                Id = result.Value,
                Name = PointName,
                Creator = new UserListModel
                {
                    Id = CurrentUserId,
                    Name = CurrentUserName
                },
                Location = LocationCoordinates.Location,
                Symbol = new Kris.Common.Models.MapPointSymbol
                {
                    Shape = MapPointShapeSelectedItem.Value,
                    Color = MapPointColorSelectedItem.Value,
                    Sign = MapPointSignSelectedItem.Value
                },
                Created = DateTime.MinValue
            })
            : Result.Fail(result.Errors);

        CreatedClosing?.Invoke(this, new ResultEventArgs<MapPointListModel>(returnResult));
    }
}
