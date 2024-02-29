using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Kris.Client.Common.Events;
using Kris.Client.Core.Requests;
using Kris.Client.Data.Models.Picker;
using Kris.Client.Data.Providers;
using MediatR;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace Kris.Client.ViewModels.Popups;

public sealed partial class CreateMapPointPopupViewModel : PopupViewModel
{
    private readonly IMapPointSymbolDataProvider _symbolDataProvider;

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

    public event EventHandler<ResultEventArgs<Guid>> CreatedClosing;

    public CreateMapPointPopupViewModel(IMapPointSymbolDataProvider symbolDataProvider, IMediator mediator)
        : base(mediator)
    {
        _symbolDataProvider = symbolDataProvider;

        _mapPointColorItems = _symbolDataProvider.GetMapPointSymbolColorItems().ToObservableCollection();
        _mapPointShapeItems = _symbolDataProvider.GetMapPointSymbolShapeItems().ToObservableCollection();
        _mapPointSignItems = _symbolDataProvider.GetMapPointSymbolSignItems().ToObservableCollection();
    }

    [RelayCommand]
    private async Task OnCreateClicked()
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
            Location = Location,
            Shape = MapPointShapeSelectedItem.Value,
            Color = MapPointColorSelectedItem.Value,
            Sign = MapPointSignSelectedItem.Value
        };
        var result = await _mediator.Send(command, ct);

        CreatedClosing?.Invoke(this, new ResultEventArgs<Guid>(result));
    }
}
