using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.Input;
using MediatR;

namespace Kris.Client.ViewModels.Popups;

public abstract partial class PopupViewModel : FormViewModelBase
{
    public event EventHandler CancelClosing;

    protected PopupViewModel(IMediator mediator, IPopupService popupService) : base(mediator, popupService)
    {
    }

    [RelayCommand]
    private void OnCancelButtonClicked() => CancelClosing?.Invoke(this, EventArgs.Empty);
}
