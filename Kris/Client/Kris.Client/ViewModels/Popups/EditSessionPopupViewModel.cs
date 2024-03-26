using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Kris.Client.Common.Errors;
using Kris.Client.Common.Events;
using Kris.Client.Components.Events;
using Kris.Client.Core.Models;
using Kris.Client.Core.Requests;
using Kris.Client.Core.Services;
using Kris.Client.Validations;
using Kris.Client.ViewModels.Utility;
using Kris.Common.Enums;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Kris.Client.ViewModels.Popups;

public sealed partial class EditSessionPopupViewModel : PopupViewModel
{
    private readonly IAlertService _alertService;

    public Guid SessionId { get; set; }

    [ObservableProperty]
    private UserType _userType;

    [Required]
    [ObservableProperty]
    private string _name;
    [ObservableProperty]
    private string _password;
    [Match("Password", "Passwords do not match")]
    [ObservableProperty]
    private string _passwordVerification;

    public event EventHandler<LoadResultEventArgs<SessionDetailModel>> LoadErrorClosing;
    public event EventHandler<UpdateResultEventArgs> UpdatedClosing;
    public event EventHandler<DeleteResultEventArgs> DeletedClosing;

    public EditSessionPopupViewModel(IAlertService alertService, IMediator mediator, IPopupService popupService)
        : base(mediator, popupService)
    {
        _alertService = alertService;
    }

    // HANDLERS
    [RelayCommand]
    private async Task OnSaveButtonClicked() => await UpdateSessionAsync();
    [RelayCommand]
    private async Task OnDeleteButtonClicked() => await DeleteSessionAsync();

    // CORE
    public async Task LoadSessionDetailAsync()
    {
        var ct = new CancellationToken();
        var query = new GetSessionDetailQuery { SessionId = SessionId };
        var result = await _mediator.Send(query, ct);

        if (result.IsFailed)
        {
            LoadErrorClosing?.Invoke(this, new LoadResultEventArgs<SessionDetailModel>(result));
        }

        Name = result.Value.Name;
    }

    private async Task UpdateSessionAsync()
    {
        if (ValidateAllProperties()) return;

        var passwordPopup = await _popupService.ShowPopupAsync<PasswordPopupViewModel>() as PasswordEventArgs;
        if (passwordPopup == null) return;

        var ct = new CancellationToken();
        var command = new EditSessionCommand { NewName = Name, NewPassword = Password, Password = passwordPopup.Password };
        var result = await _mediator.Send(command, ct);

        if (result.IsFailed && result.HasError<ForbiddenError>())
        {
            await _alertService.ShowToastAsync("Wrong password");
            return;
        }

        UpdatedClosing?.Invoke(this, new UpdateResultEventArgs(result));
    }

    private async Task DeleteSessionAsync()
    {
        var passwordPopup = await _popupService.ShowPopupAsync<PasswordPopupViewModel>() as PasswordEventArgs;
        if (passwordPopup == null) return;

        var ct = new CancellationToken();
        var command = new EndSessionCommand { Password = passwordPopup.Password };
        var result = await _mediator.Send(command, ct);

        if (result.IsFailed && result.HasError<ForbiddenError>())
        {
            await _alertService.ShowToastAsync("Wrong password");
            return;
        }

        DeletedClosing?.Invoke(this, new DeleteResultEventArgs(result));
    }
}
