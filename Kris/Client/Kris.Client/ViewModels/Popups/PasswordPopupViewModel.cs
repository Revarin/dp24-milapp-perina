using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Kris.Client.Components.Events;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Kris.Client.ViewModels.Popups;

public sealed partial class PasswordPopupViewModel : PopupViewModel
{
    [Required]
    [ObservableProperty]
    private string _password;

    public event EventHandler<PasswordEventArgs> AcceptedClosing;

    public PasswordPopupViewModel(IMediator mediator) : base(mediator)
    {
    }

    [RelayCommand]
    private void OnOkButtonClicked()
    {
        if (ValidateAllProperties()) return;
        AcceptedClosing?.Invoke(this, new PasswordEventArgs(Password));
    }
}
