﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Kris.Client.Common.Errors;
using Kris.Client.Common.Utility;
using Kris.Client.Core.Requests;
using Kris.Client.Events;
using Kris.Client.Validations;
using Kris.Client.Views;
using Kris.Common.Extensions;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Kris.Client.ViewModels.Popups;

public sealed partial class EditSessionPopupViewModel : PopupViewModel
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

    public EditSessionPopupViewModel(IMediator mediator)
        : base(mediator)
    {
    }

    [RelayCommand]
    private async Task OnCreateClicked()
    {
        if (ValidateAllProperties()) return;

        var ct = new CancellationToken();
        var command = new CreateSessionCommand { Name = Name, Password = Password };
        var result = await _mediator.Send(command, ct);

        if (result.IsFailed && result.HasError<EntityExistsError>())
        {
            AddCustomError(nameof(Name), "Session already exists");
            return;
        }

        CreatedClosing?.Invoke(this, new ResultEventArgs(result));
    }
}
