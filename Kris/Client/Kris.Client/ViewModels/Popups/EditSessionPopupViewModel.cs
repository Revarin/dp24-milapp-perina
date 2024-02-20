using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Kris.Client.Core.Services;
using Kris.Client.Validations;
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

    public EditSessionPopupViewModel(IMediator mediator, IRouterService navigationService, IAlertService alertService)
        : base(mediator, navigationService, alertService)
    {
    }

    [RelayCommand]
    private Task OnCancelClicked()
    {
        throw new NotImplementedException();
    }

    [RelayCommand]
    private Task OnCreateClicked()
    {
        throw new NotImplementedException();
    }
}
