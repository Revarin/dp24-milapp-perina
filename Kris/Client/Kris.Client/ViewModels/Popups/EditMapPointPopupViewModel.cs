using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FluentResults;
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
    public Guid CurrentUserId { get; set; }
    public string CurrentUserName { get; set; }
    public UserType CurrentUserType { get; set; }

    [Required]
    [ObservableProperty]
    private string _pointName;
    [ObservableProperty]
    private string _description;
    [ObservableProperty]
    private Location _location;
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
    private ImageSource _image;
    [ObservableProperty]
    private bool _canEdit;

    public event EventHandler<LoadResultEventArgs<MapPointDetailModel>> LoadErrorClosing;
    public event EventHandler<UpdateResultEventArgs<MapPointListModel>> UpdatedClosing;
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

    // HANDLERS
    [RelayCommand]
    private void OnSymbolComponentChanged() => RedrawSymbol();
    [RelayCommand]
    private async Task OnSaveButtonClicked() => await UpdateMapPointAsync();
    [RelayCommand]
    private async Task OnDeleteButtonClicked() => await DeleteMapPointAsync();

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
        var result = await _mediator.Send(query, ct);

        if (result.IsFailed)
        {
            LoadErrorClosing?.Invoke(this, new LoadResultEventArgs<MapPointDetailModel>(result));
        }

        var mapPoint = result.Value;

        PointName = mapPoint.Name;
        Description = mapPoint.Description;
        Location = mapPoint.Location;
        Created = mapPoint.Created;
        MapPointShapeSelectedItem = MapPointShapeItems.FirstOrDefault(shape => shape.Value == mapPoint.Symbol.Shape);
        MapPointColorSelectedItem = MapPointColorItems.FirstOrDefault(color => color.Value == mapPoint.Symbol.Color);
        MapPointSignSelectedItem = MapPointSignItems.FirstOrDefault(sign => sign.Value == mapPoint.Symbol.Sign);

        CanEdit = CurrentUserType >= UserType.Admin || mapPoint.Creator.Id == CurrentUserId;
    }

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
                Location = Location,
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
        var ct = new CancellationToken();
        var command = new DeleteMapPointCommand { Id = PointId };
        var result = await _mediator.Send(command, ct);

        DeletedClosing?.Invoke(this, new DeleteResultEventArgs(result));
    }
}
