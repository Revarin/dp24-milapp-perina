using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Kris.Client.Common.Errors;
using Kris.Client.Common.Events;
using Kris.Client.Core.Requests;
using Kris.Client.Validations;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Kris.Client.ViewModels.Popups;

public sealed partial class CreateSessionPopupViewModel : PopupViewModel
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

    public event EventHandler<ResultEventArgs> CreatedClosing;

    public CreateSessionPopupViewModel(IMediator mediator, IPopupService popupService)
        : base(mediator, popupService)
    {
    }

    // HANDLERS
    [RelayCommand]
    private async Task OnCreateButtonClicked() => await CreateSessionAsync();

    // CORE
    private async Task CreateSessionAsync()
    {
        if (ValidateAllProperties()) return;

        var ct = new CancellationToken();
        var command = new CreateSessionCommand { Name = Name, Password = Password };
        var result = await MediatorSendAsync(command, ct);

        if (result.IsFailed && result.HasError<EntityExistsError>())
        {
            AddCustomError(nameof(Name), "Session already exists");
            return;
        }

        CreatedClosing?.Invoke(this, new ResultEventArgs(result));
    }
}
