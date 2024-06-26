﻿using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Kris.Client.Common.Errors;
using Kris.Client.Common.Events;
using Kris.Client.Components.Events;
using Kris.Client.Core.Models;
using Kris.Client.Core.Requests;
using Kris.Client.Core.Services;
using Kris.Client.Data.Models.Picker;
using Kris.Client.Data.Providers;
using Kris.Client.Utility;
using Kris.Client.Validations;
using Kris.Client.ViewModels.Items;
using Kris.Client.ViewModels.Utility;
using Kris.Common.Enums;
using Kris.Common.Models;
using MediatR;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace Kris.Client.ViewModels.Popups;

public sealed partial class EditSessionPopupViewModel : PopupViewModel
{
    private readonly IMapPointSymbolDataProvider _symbolDataProvider;
    private readonly ISymbolImageComposer _symbolImageComposer;
    private readonly IAlertService _alertService;

    public event EventHandler<LoadResultEventArgs<SessionDetailModel>> LoadErrorClosing;
    public event EventHandler<UpdateResultEventArgs> UpdatedClosing;
    public event EventHandler<DeleteResultEventArgs> DeletedClosing;

    public Guid SessionId { get; set; }

    [ObservableProperty]
    private UserType _userType;
    [Required]
    [ObservableProperty]
    private string _userName;
    [ObservableProperty]
    private ObservableCollection<MapPointSymbolColorItem> _mapPointColorItems;
    [Required]
    [ObservableProperty]
    private MapPointSymbolColorItem _mapPointColorSelectedItem;
    [ObservableProperty]
    private ObservableCollection<MapPointSymbolShapeItem> _mapPointShapeItems;
    [Required]
    [ObservableProperty]
    private MapPointSymbolShapeItem _mapPointShapeSelectedItem;
    [ObservableProperty]
    private ObservableCollection<MapPointSymbolSignItem> _mapPointSignItems;
    [Required]
    [ObservableProperty]
    private MapPointSymbolSignItem _mapPointSignSelectedItem;

    [ObservableProperty]
    private ObservableCollection<SessionUserItemViewModel> _sessionUsers;

    [Required]
    [ObservableProperty]
    private string _sessionName;
    [ObservableProperty]
    private string _password;
    [Match("Password", "Passwords do not match")]
    [ObservableProperty]
    private string _passwordVerification;

    [ObservableProperty]
    private ImageSource _image;

    public EditSessionPopupViewModel(IMapPointSymbolDataProvider symbolDataProvider, ISymbolImageComposer symbolImageComposer,
        IAlertService alertService, IMediator mediator, IPopupService popupService)
        : base(mediator, popupService)
    {
        _symbolDataProvider = symbolDataProvider;
        _symbolImageComposer = symbolImageComposer;
        _alertService = alertService;

        MapPointColorItems = _symbolDataProvider.GetMapPointSymbolColorItems().ToObservableCollection();
        MapPointShapeItems = _symbolDataProvider.GetMapPointSymbolShapeItems().ToObservableCollection();
        MapPointSignItems = _symbolDataProvider.GetMapPointSymbolSignItems().ToObservableCollection();
    }

    // HANDLERS
    [RelayCommand]
    private void OnSymbolComponentChanged() => RedrawSymbol();
    [RelayCommand]
    private async Task OnSaveButtonClicked() => await UpdateSessionUserAsync();
    [RelayCommand]
    private async Task OnAdminSaveButtonClicked() => await UpdateSessionAsync();
    [RelayCommand]
    private async Task OnDeleteButtonClicked() => await DeleteSessionAsync();
    private async void OnUserRoleChanging(object sender, EventArgs e)
    {
        if (sender is SessionUserItemViewModel sessionUser)
        {
            await UpdateSessionUserRole(sessionUser.Id, sessionUser.UserType);
        }
    }
    private async void OnUserKicking(object sender, EventArgs e)
    {
        if (sender is SessionUserItemViewModel sessionUser)
        {
            await KickUserAsync(sessionUser.Id);
        }
    }

    // CORE
    public async Task LoadSessionDetailAsync()
    {
        var ct = new CancellationToken();
        var query = new GetSessionDetailQuery { SessionId = SessionId };
        var result = await _mediator.Send(query, ct);

        if (result.IsFailed)
        {
            LoadErrorClosing?.Invoke(this, new LoadResultEventArgs<SessionDetailModel>(result));
            return;
        }

        SessionName = result.Value.Name;
        UserName = result.Value.UserName;
        MapPointShapeSelectedItem = MapPointShapeItems.First(shape => shape.Value == result.Value.UserSymbol.Shape);
        MapPointColorSelectedItem = MapPointColorItems.First(color => color.Value == result.Value.UserSymbol.Color);
        MapPointSignSelectedItem = MapPointSignItems.First(sign => sign.Value == result.Value.UserSymbol.Sign);

        SessionUsers = result.Value.Users.Select(user => new SessionUserItemViewModel(user)).ToObservableCollection();
        foreach (var sessionUser in SessionUsers)
        {
            sessionUser.UserRoleChanging += OnUserRoleChanging;
            sessionUser.UserKicking += OnUserKicking;
        }
    }

    private async Task UpdateSessionUserAsync()
    {
        if (ValidateUserProperties()) return;

        var ct = new CancellationToken();
        var command = new EditSessionUserCommand
        {
            UserName = UserName,
            UserSymbol = new MapPointSymbol
            {
                Shape = MapPointShapeSelectedItem.Value,
                Color = MapPointColorSelectedItem.Value,
                Sign = MapPointSignSelectedItem.Value
            }
        };
        var result = await _mediator.Send(command, ct);

        UpdatedClosing?.Invoke(this, new UpdateResultEventArgs(result));
    }

    private async Task UpdateSessionAsync()
    {
        if (ValidateAdminProperties()) return;

        var passwordPopup = await _popupService.ShowPopupAsync<PasswordPopupViewModel>() as PasswordEventArgs;
        if (passwordPopup == null) return;

        var ct = new CancellationToken();
        var command = new EditSessionCommand { NewName = SessionName, NewPassword = Password, Password = passwordPopup.Password };
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

    private async Task UpdateSessionUserRole(Guid userId, UserType newRole)
    {
        var ct = new CancellationToken();
        var command = new EditSessionUserRoleCommand { UserId = userId, NewRole = newRole };
        var result = await MediatorSendAsync(command, ct);

        if (result.IsFailed)
        {
            if (result.HasError<ForbiddenError>())
            {
                await _alertService.ShowToastAsync("Cannot change role of this user");
                SessionUsers.FirstOrDefault(su => su.Id == userId)?.RevertUserType(newRole);
            }
            else if (result.HasError<ConnectionError>())
            {
                await _alertService.ShowToastAsync("No connection to server");
                SessionUsers.FirstOrDefault(su => su.Id == userId)?.RevertUserType(newRole);
            }
            else
            {
                UpdatedClosing?.Invoke(this, new UpdateResultEventArgs(result));
            }
        }
    }

    private async Task KickUserAsync(Guid userId)
    {
        var confirmation = await _popupService.ShowPopupAsync<ConfirmationPopupViewModel>(vm => vm.Message = "Kick user?") as ConfirmationEventArgs;
        if (confirmation == null || !confirmation.IsConfirmed) return;

        var ct = new CancellationToken();
        var command = new KickSessionUserCommand { UserId = userId };
        var result = await MediatorSendAsync(command, ct);

        if (result.IsFailed)
        {
            if (result.HasError<ForbiddenError>())
            {
                await _alertService.ShowToastAsync("Cannot kick this user");
            }
            else if (result.HasError<ConnectionError>())
            {
                await _alertService.ShowToastAsync("No connection to server");
            }
            else
            {
                UpdatedClosing?.Invoke(this, new UpdateResultEventArgs(result));
            }
        }
        else
        {
            var sessionUser = SessionUsers.FirstOrDefault(su => su.Id == userId);
            SessionUsers.Remove(sessionUser);
        }
    }

    // MISC
    private void RedrawSymbol()
    {
        var pointShape = MapPointShapeSelectedItem?.Value;
        var pointColor = MapPointColorSelectedItem?.Value;
        var pointSign = MapPointSignSelectedItem?.Value;

        var imageStream = _symbolImageComposer.ComposeMapPointSymbol(pointShape, pointColor, pointSign);
        Image = ImageSource.FromStream(() => imageStream);
    }

    private bool ValidateAdminProperties()
    {
        ErrorMessages.Clear();
        ValidateProperty(SessionName, nameof(SessionName));
        ValidateProperty(Password, nameof(Password));
        ValidateProperty(PasswordVerification, nameof(PasswordVerification));

        AddErrors();

        OnPropertyChanged(nameof(ErrorMessages));
        return HasErrors;
    }

    private bool ValidateUserProperties()
    {
        ErrorMessages.Clear();
        ValidateProperty(UserName, nameof(UserName));
        ValidateProperty(MapPointShapeSelectedItem, nameof(MapPointShapeSelectedItem));
        ValidateProperty(MapPointColorSelectedItem, nameof(MapPointColorSelectedItem));
        ValidateProperty(MapPointSignSelectedItem, nameof(MapPointSignSelectedItem));

        AddErrors();

        OnPropertyChanged(nameof(ErrorMessages));
        return HasErrors;
    }
}
