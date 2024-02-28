using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Kris.Client.Common.Events;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Kris.Client.ViewModels.Popups;

public sealed partial class CreateMapPointPopupViewModel : PopupViewModel
{
    [Required]
    [ObservableProperty]
    private string _pointName;
    [ObservableProperty]
    private string _description;

    public event EventHandler<ResultEventArgs> CreatedClosing;

    public CreateMapPointPopupViewModel(IMediator mediator)
        : base(mediator)
    {
    }

    [RelayCommand]
    private Task OnCreateClicked()
    {
        throw new NotImplementedException();
    }
}
