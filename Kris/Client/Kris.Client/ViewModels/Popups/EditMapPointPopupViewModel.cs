using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Kris.Client.Common.Events;
using Kris.Client.Components.Map;
using Kris.Client.Core.Models;
using Kris.Client.Core.Requests;
using Kris.Client.Data.Models.Picker;
using Kris.Client.Data.Providers;
using Kris.Client.Utility;
using Kris.Common.Enums;
using MediatR;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace Kris.Client.ViewModels.Popups;

public sealed partial class EditMapPointPopupViewModel : PopupViewModel
{
    private readonly IMapPointSymbolDataProvider _symbolDataProvider;
    private readonly ISymbolImageComposer _symbolImageComposer;

    public Guid PointId { get; set; }

    [ObservableProperty]
    private bool _isCreator;
    [ObservableProperty]
    private UserType _userType;

    [Required]
    [ObservableProperty]
    private string _pointName;
    [ObservableProperty]
    private string _description;
    [ObservableProperty]
    private Location _location;

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

    public event EventHandler<UpdateResultEventArgs> UpdatedClosing;
    public event EventHandler<DeleteResultEventArgs> DeletedClosing;

    public EditMapPointPopupViewModel(IMapPointSymbolDataProvider mapPointSymbolDataProvider, ISymbolImageComposer symbolImageComposer,
        IMediator mediator)
        : base(mediator)
    {
        _symbolDataProvider = mapPointSymbolDataProvider;
        _symbolImageComposer = symbolImageComposer;

        _mapPointColorItems = _symbolDataProvider.GetMapPointSymbolColorItems().ToObservableCollection();
        _mapPointShapeItems = _symbolDataProvider.GetMapPointSymbolShapeItems().ToObservableCollection();
        _mapPointSignItems = _symbolDataProvider.GetMapPointSymbolSignItems().ToObservableCollection();
    }

    public void Initialize(CurrentUserModel user, KrisMapPin pin)
    {
        // TODO: Rework
        UserType = user.UserType.Value;
        IsCreator = user.Id == pin.CreatorId;

        PointId = pin.KrisId;
        PointName = pin.Label;
        Location = pin.Location;
        Description = pin.Description;
    }

    // HANDLERS
    [RelayCommand]
    private void OnSymbolComponentChanged() => RedrawSymbol();
    [RelayCommand]
    private async Task OnSaveButtonClicked() => await UpdateMapPointAsync();
    [RelayCommand]
    private async Task OnDeleteButtonClicked() => await DeleteMapPointAsync();

    // CORE
    private void RedrawSymbol()
    {
        var pointShape = MapPointShapeSelectedItem?.Value;
        var pointColor = MapPointColorSelectedItem?.Value;
        var pointSign = MapPointSignSelectedItem?.Value;

        var imageStream = _symbolImageComposer.ComposeMapPointSymbol(pointShape, pointColor, pointSign);
        Image = ImageSource.FromStream(() => imageStream);
    }

    private async Task UpdateMapPointAsync()
    {
        if (ValidateAllProperties()) return;

        var ct = new CancellationToken();
        var command = new EditMapPointCommand
        {
            PointId = PointId,
            Name = PointName,
            Description = Description,
            Location = Location,
            Shape = MapPointShapeSelectedItem.Value,
            Color = MapPointColorSelectedItem.Value,
            Sign = MapPointSignSelectedItem.Value
        };
        var result = await _mediator.Send(command, ct);

        UpdatedClosing?.Invoke(this, new UpdateResultEventArgs(result));
    }

    private async Task DeleteMapPointAsync()
    {
        var ct = new CancellationToken();
        var command = new DeleteMapPointCommand { Id = PointId };
        var result = await _mediator.Send(command, ct);

        DeletedClosing?.Invoke(this, new DeleteResultEventArgs(result));
    }
}
