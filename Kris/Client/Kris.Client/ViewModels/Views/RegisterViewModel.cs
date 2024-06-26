﻿using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Kris.Client.Common.Errors;
using Kris.Client.Core.Requests;
using Kris.Client.Core.Services;
using Kris.Client.Validations;
using Kris.Common.Extensions;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Kris.Client.ViewModels.Views;

public sealed partial class RegisterViewModel : PageViewModelBase
{
    [Required]
    [ObservableProperty]
    private string _login;
    [Required]
    [ObservableProperty]
    private string _password;
    [Required]
    [Match("Password", "Passwords do not match")]
    [ObservableProperty]
    private string _passwordVerification;

    public RegisterViewModel(IMediator mediator, IRouterService navigationService, IMessageService messageService, IPopupService popupService, IAlertService alertService)
        : base(mediator, navigationService, messageService, popupService, alertService)
    {
    }

    // HANDLERS
    [RelayCommand]
    private async Task OnRegisterButtonClicked() => await RegisterUserAsync();

    // CORE
    private async Task RegisterUserAsync()
    {
        if (ValidateAllProperties()) return;

        var ct = new CancellationToken();
        var command = new RegisterUserCommand { Login = Login, Password = Password };
        var result = await MediatorSendLoadingAsync(command, ct);

        if (result.IsFailed)
        {
            if (result.HasError<EntityExistsError>())
            {
                AddCustomError(nameof(Login), "User already exists");
            }
            else if (result.HasError<ConnectionError>())
            {
                await _alertService.ShowToastAsync("No connection to server");
            }
            else
            {
                await _alertService.ShowToastAsync(result.Errors.FirstMessage());
            }
        }
        else
        {
            Cleanup();
            await _alertService.ShowToastAsync("User registered");
            await _navigationService.GoToAsync("..");
        }
    }

    // MISC
    protected override void Cleanup()
    {
        Login = string.Empty;
        Password = string.Empty;
        PasswordVerification = string.Empty;

        base.Cleanup();
    }
}
