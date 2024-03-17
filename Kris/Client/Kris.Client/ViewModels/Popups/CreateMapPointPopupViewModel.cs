using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CoordinateSharp;
using FluentResults;
using Kris.Client.Common.Enums;
using Kris.Client.Common.Events;
using Kris.Client.Converters;
using Kris.Client.Core.Models;
using Kris.Client.Core.Requests;
using Kris.Client.Core.Services;
using Kris.Client.Data.Models.Picker;
using Kris.Client.Data.Providers;
using Kris.Client.Utility;
using Kris.Client.ViewModels.Items;
using Kris.Common.Enums;
using MediatR;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace Kris.Client.ViewModels.Popups;

public sealed partial class CreateMapPointPopupViewModel : PopupViewModel
{
    private readonly IMapSettingsDataProvider _mapSettingsDataProvider;
    private readonly IMapPointSymbolDataProvider _symbolDataProvider;
    private readonly ISymbolImageComposer _symbolImageComposer;
    private readonly IFilePickerService _filePickerService;
    private readonly IClipboardService _clipboardService;

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

    [ObservableProperty]
    private ObservableCollection<ImageItemViewModel> _imageAttachments = new ObservableCollection<ImageItemViewModel>();

    public event EventHandler<ResultEventArgs<MapPointListModel>> CreatedClosing;

    public CreateMapPointPopupViewModel(IMapSettingsDataProvider mapSettingsDataProvider, IMapPointSymbolDataProvider symbolDataProvider,
        ISymbolImageComposer symbolImageComposer, IFilePickerService filePickerService, IClipboardService clipboardService,
        IMediator mediator)
        : base(mediator)
    {
        _mapSettingsDataProvider = mapSettingsDataProvider;
        _symbolDataProvider = symbolDataProvider;
        _symbolImageComposer = symbolImageComposer;
        _filePickerService = filePickerService;
        _clipboardService = clipboardService;

        MapPointColorItems = _symbolDataProvider.GetMapPointSymbolColorItems().ToObservableCollection();
        MapPointShapeItems = _symbolDataProvider.GetMapPointSymbolShapeItems().ToObservableCollection();
        MapPointSignItems = _symbolDataProvider.GetMapPointSymbolSignItems().ToObservableCollection();
    }

    // HANDLERS
    [RelayCommand]
    private void OnSymbolComponentChanged() => RedrawSymbol();
    [RelayCommand]
    private async Task OnCoordinatesCopyButtonClicked() => await SaveLocationCoordinatesToClipboardAsync();
    [RelayCommand]
    private async Task OnAddAttachmentButtonClicked() => await PickAttachmentAsync();
    [RelayCommand]
    private async Task OnCreateButtonClicked() => await CreateMapPointAsync();
    private void OnImageAttachmentDeleteClicked(object sender, EventArgs e) => RemoveAttachment(sender as ImageItemViewModel);

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

    private async Task SaveLocationCoordinatesToClipboardAsync()
    {
        var coordinate = new Coordinate(LocationCoordinates.Location.Latitude, LocationCoordinates.Location.Longitude);
        string coordinateString;

        switch (LocationCoordinates.CoordinateSystem)
        {
            case CoordinateSystem.LatLong:
                coordinateString = coordinate.ToString();
                break;
            case CoordinateSystem.UTM:
                coordinateString = coordinate.UTM.ToString();
                break;
            case CoordinateSystem.MGRS:
            default:
                coordinateString = coordinate.MGRS.ToString();
                break;
        }

        await _clipboardService.SetAsync(coordinateString);
    }

    private async Task PickAttachmentAsync()
    {
        var fileResult = await _filePickerService.PickImageAsync();
        if (fileResult == null) return;

        var imageItem = new ImageItemViewModel(await fileResult.OpenReadAsync(), true);
        imageItem.DeleteClicked += OnImageAttachmentDeleteClicked;
        ImageAttachments.Add(imageItem);
    }

    private void RemoveAttachment(ImageItemViewModel image)
    {
        if (image == null) return;
        image.DeleteClicked -= OnImageAttachmentDeleteClicked;
        ImageAttachments.Remove(image);
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
