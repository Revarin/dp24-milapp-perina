using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CoordinateSharp;
using FluentResults;
using Kris.Client.Common.Events;
using Kris.Client.Converters;
using Kris.Client.Core.Models;
using Kris.Client.Core.Requests;
using Kris.Client.Core.Services;
using Kris.Client.Data.Models.Picker;
using Kris.Client.Data.Providers;
using Kris.Client.Utility;
using Kris.Client.ViewModels.Items;
using Kris.Client.ViewModels.Utility;
using Kris.Common.Enums;
using MediatR;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace Kris.Client.ViewModels.Popups;

public sealed partial class EditMapPointPopupViewModel : PopupViewModel
{
    private readonly IMapSettingsDataProvider _mapSettingsDataProvider;
    private readonly IMapPointSymbolDataProvider _symbolDataProvider;
    private readonly ISymbolImageComposer _symbolImageComposer;
    private readonly IMediaService _filePickerService;
    private readonly IClipboardService _clipboardService;

    public Guid PointId { get; set; }
    public Guid CurrentUserId { get; set; }
    public string CurrentUserName { get; set; }
    public UserType CurrentUserType { get; set; }

    [Required]
    [ObservableProperty]
    private string _pointName;
    [ObservableProperty]
    private string _description;
    [ObservableProperty]
    private LocationCoordinates _locationCoordinates;
    [ObservableProperty]
    private DateTime _created;

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
    private ObservableCollection<ImageItemViewModel> _imageAttachments = new ObservableCollection<ImageItemViewModel>();

    [ObservableProperty]
    private ImageSource _image;
    [ObservableProperty]
    private bool _canEdit;

    public event EventHandler<LoadResultEventArgs<MapPointDetailModel>> LoadErrorClosing;
    public event EventHandler<UpdateResultEventArgs<MapPointListModel>> UpdatedClosing;
    public event EventHandler<DeleteResultEventArgs> DeletedClosing;

    private List<Guid> _attachmentsToDelete = new List<Guid>();

    public EditMapPointPopupViewModel(IMapSettingsDataProvider mapSettingsDataProvider, IMapPointSymbolDataProvider mapPointSymbolDataProvider,
        ISymbolImageComposer symbolImageComposer, IMediaService filePickerService, IClipboardService clipboardService,
        IMediator mediator, IPopupService popupService)
        : base(mediator, popupService)
    {
        _mapSettingsDataProvider = mapSettingsDataProvider;
        _symbolDataProvider = mapPointSymbolDataProvider;
        _symbolImageComposer = symbolImageComposer;
        _filePickerService = filePickerService;
        _clipboardService = clipboardService;

        _mapPointColorItems = _symbolDataProvider.GetMapPointSymbolColorItems().ToObservableCollection();
        _mapPointShapeItems = _symbolDataProvider.GetMapPointSymbolShapeItems().ToObservableCollection();
        _mapPointSignItems = _symbolDataProvider.GetMapPointSymbolSignItems().ToObservableCollection();
    }

    // HANDLERS
    [RelayCommand]
    private void OnSymbolComponentChanged() => RedrawSymbol();
    [RelayCommand]
    private async Task OnCoordinatesCopyButtonClicked() => await SaveLocationCoordinatesToClipboardAsync();
    [RelayCommand]
    private async Task OnPickImageAttachmentButtonClicked() => await PickImageAsync();
    [RelayCommand]
    private async Task OnTakeImageAttachmentButtonClicked() => await TakeImageAsync();

    [RelayCommand]
    private async Task OnSaveButtonClicked() => await UpdateMapPointAsync();
    [RelayCommand]
    private async Task OnDeleteButtonClicked() => await DeleteMapPointAsync();
    private async void OnImageAttachmentDeleteClicked(object sender, EventArgs e) => await RemoveAttachmentAsync(sender as ImageItemViewModel);

    // CORE
    public void Setup(Guid pointId, Guid currentUserId, string currentUserName, UserType currentUserType)
    {
        PointId = pointId;
        CurrentUserId = currentUserId;
        CurrentUserName = currentUserName;
        CurrentUserType = currentUserType;
    }

    public async Task LoadMapPointDetailAsync()
    {
        var ct = new CancellationToken();
        var query = new GetMapPointDetailQuery { PointId = PointId };
        var result = await MediatorSendLoadingAsync(query, ct);

        if (result.IsFailed)
        {
            LoadErrorClosing?.Invoke(this, new LoadResultEventArgs<MapPointDetailModel>(result));
        }

        var mapPoint = result.Value;

        CanEdit = CurrentUserType >= UserType.Admin || mapPoint.Creator.Id == CurrentUserId;

        PointName = mapPoint.Name;
        Description = mapPoint.Description;
        LocationCoordinates = new LocationCoordinates
        {
            Location = mapPoint.Location,
            CoordinateSystem = _mapSettingsDataProvider.GetCurrentCoordinateSystem().Value
        };
        Created = mapPoint.Created;
        MapPointShapeSelectedItem = MapPointShapeItems.FirstOrDefault(shape => shape.Value == mapPoint.Symbol.Shape);
        MapPointColorSelectedItem = MapPointColorItems.FirstOrDefault(color => color.Value == mapPoint.Symbol.Color);
        MapPointSignSelectedItem = MapPointSignItems.FirstOrDefault(sign => sign.Value == mapPoint.Symbol.Sign);

        foreach (var attachment in mapPoint.Attachments)
        {
            var imageItem = new ImageItemViewModel(_popupService, attachment.Base64Bytes, attachment.Id, attachment.Name, CanEdit);
            imageItem.DeleteClicked += OnImageAttachmentDeleteClicked;
            ImageAttachments.Add(imageItem);
        }
    }

    private void RedrawSymbol()
    {
        var pointShape = MapPointShapeSelectedItem?.Value;
        var pointColor = MapPointColorSelectedItem?.Value;
        var pointSign = MapPointSignSelectedItem?.Value;

        var imageStream = _symbolImageComposer.ComposeMapPointSymbol(pointShape, pointColor, pointSign);
        Image = ImageSource.FromStream(() => imageStream);
    }

    private async Task PickImageAsync()
    {
        var fileResult = await _filePickerService.PickImageAsync();
        if (fileResult == null) return;
        AddImageAttachment(fileResult);
    }

    private async Task TakeImageAsync()
    {
        var fileResult = await _filePickerService.TakePhotoAsync();
        if (fileResult == null) return;
        AddImageAttachment(fileResult);
    }

    private async Task RemoveAttachmentAsync(ImageItemViewModel image)
    {
        var confirmation = await _popupService.ShowPopupAsync<ConfirmationPopupViewModel>(vm => vm.Message = "Remove attachment?") as ConfirmationEventArgs;
        if (confirmation == null || !confirmation.IsConfirmed) return;

        if (image == null) return;
        if (image.Id.HasValue) _attachmentsToDelete.Add(image.Id.Value);

        image.DeleteClicked -= OnImageAttachmentDeleteClicked;
        ImageAttachments.Remove(image);
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

    private async Task UpdateMapPointAsync()
    {
        if (ValidateAllProperties()) return;

        var confirmation = await _popupService.ShowPopupAsync<ConfirmationPopupViewModel>(vm => vm.Message = "Update point?") as ConfirmationEventArgs;
        if (confirmation == null || !confirmation.IsConfirmed) return;

        var ct = new CancellationToken();
        var command = new EditMapPointCommand
        {
            PointId = PointId,
            Name = PointName,
            Description = Description,
            Location = LocationCoordinates.Location,
            Shape = MapPointShapeSelectedItem.Value,
            Color = MapPointColorSelectedItem.Value,
            Sign = MapPointSignSelectedItem.Value,
            DeletedAttachments = _attachmentsToDelete,
            NewAttachments = ImageAttachments.Where(attachment => !attachment.Id.HasValue).Select(attachment => attachment.FilePath).ToList()
        };
        var result = await MediatorSendAsync(command, ct);

        var returnResult = result.IsSuccess
            ? Result.Ok(new MapPointListModel
            {
                Id = PointId,
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
            : result;

        UpdatedClosing?.Invoke(this, new UpdateResultEventArgs<MapPointListModel>(returnResult));
    }

    private async Task DeleteMapPointAsync()
    {
        var confirmation = await _popupService.ShowPopupAsync<ConfirmationPopupViewModel>(vm => vm.Message = "Delete point?") as ConfirmationEventArgs;
        if (confirmation == null || !confirmation.IsConfirmed) return;

        var ct = new CancellationToken();
        var command = new DeleteMapPointCommand { Id = PointId };
        var result = await MediatorSendAsync(command, ct);

        DeletedClosing?.Invoke(this, new DeleteResultEventArgs(result));
    }

    // MISC
    private void AddImageAttachment(FileResult result)
    {
        var imageItem = new ImageItemViewModel(_popupService, result.FullPath, true);
        imageItem.DeleteClicked += OnImageAttachmentDeleteClicked;
        ImageAttachments.Add(imageItem);
    }
}
