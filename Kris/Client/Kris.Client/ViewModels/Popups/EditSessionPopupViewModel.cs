using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Kris.Client.Common.Errors;
using Kris.Client.Common.Utility;
using Kris.Client.Core.Requests;
using Kris.Client.Core.Services;
using Kris.Client.Validations;
using Kris.Client.Views;
using Kris.Common.Extensions;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Kris.Client.ViewModels.Popups;

public sealed partial class EditSessionPopupViewModel : ViewModelBase
{
    [Required]
    [ObservableProperty]
    private string _name;
    [Required]
    [ObservableProperty]
    private string _password;
    [Required]
    [Match("Password", "Passwords do not match")]
    [ObservableProperty]
    private string _passwordVerification;

    public event EventHandler RaiseClosePopupEvent;

    public EditSessionPopupViewModel(IMediator mediator, IRouterService navigationService, IAlertService alertService)
        : base(mediator, navigationService, alertService)
    {
    }

    [RelayCommand]
    private void OnCancelClicked() => RaiseClosePopupEvent.Invoke(this, null);

    [RelayCommand]
    private async Task OnCreateClicked()
    {
        if (ValidateAllProperties()) return;

        var ct = new CancellationToken();
        var command = new CreateSessionCommand { Name = Name, Password = Password };
        var result = await _mediator.Send(command, ct);

        if (result.IsFailed)
        {
            if (result.HasError<EntityExistsError>())
            {
                AddCustomError(nameof(Name), "Session already exists");
            }
            else if (result.HasError<UnauthorizedError>())
            {
                await _alertService.ShowToastAsync("Login expired");
                await _mediator.Send(new LogoutUserCommand(), ct);
                await _navigationService.GoToAsync(nameof(LoginView), RouterNavigationType.ReplaceUpward);
                RaiseClosePopupEvent.Invoke(this, null);
            }
            else
            {
                await _alertService.ShowToastAsync(result.Errors.FirstMessage());
            }
        }
        else
        {
            await _alertService.ShowToastAsync("Session created");
            RaiseClosePopupEvent.Invoke(this, null);
        }
    }
}
