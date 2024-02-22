using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Kris.Client.Events;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Kris.Client.ViewModels.Popups;

public sealed partial class PasswordPopupViewModel : PopupViewModel
{
    [Required]
    [ObservableProperty]
    private string _password;

    public PasswordPopupViewModel(IMediator mediator) : base(mediator)
    {
    }

    public event EventHandler<PasswordEventArgs> AcceptedClosing;

    [RelayCommand]
    private void OnOkClicked()
    {
        if (ValidateAllProperties()) return;
        AcceptedClosing?.Invoke(this, new PasswordEventArgs(Password));
    }
}
