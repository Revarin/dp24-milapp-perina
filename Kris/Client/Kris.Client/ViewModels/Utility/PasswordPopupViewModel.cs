using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Kris.Client.Components.Events;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Kris.Client.ViewModels.Utility;

public sealed partial class PasswordPopupViewModel : FormViewModelBase
{
    public event EventHandler CancelClosing;
    public event EventHandler<PasswordEventArgs> AcceptedClosing;

    [Required]
    [ObservableProperty]
    private string _password;

    public PasswordPopupViewModel(IMediator mediator, IPopupService popupService) : base(mediator, popupService)
    {
    }

    [RelayCommand]
    private void OnOkButtonClicked()
    {
        if (ValidateAllProperties()) return;
        AcceptedClosing?.Invoke(this, new PasswordEventArgs(Password));
    }

    [RelayCommand]
    private void OnCancelButtonClicked() => CancelClosing?.Invoke(this, EventArgs.Empty);
}
