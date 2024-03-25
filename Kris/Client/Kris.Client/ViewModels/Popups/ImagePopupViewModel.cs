using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using MediatR;

namespace Kris.Client.ViewModels.Popups;

public sealed partial class ImagePopupViewModel : PopupViewModel
{
    [ObservableProperty]
    private ImageSource _imageSource;

    public ImagePopupViewModel(IMediator mediator, IPopupService popupService) : base(mediator, popupService)
    {
    }
}
