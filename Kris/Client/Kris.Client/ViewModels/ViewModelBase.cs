using CommunityToolkit.Mvvm.ComponentModel;
using Kris.Client.Core.Services;
using MediatR;

namespace Kris.Client.ViewModels;

public abstract partial class ViewModelBase : ObservableValidator
{
    protected readonly IMediator _mediator;
    protected readonly IRouterService _navigationService;
    protected readonly IAlertService _alertService;

    [ObservableProperty]
    protected Dictionary<string, string> errorMessages = new Dictionary<string, string>();

    public ViewModelBase(IMediator mediator, IRouterService navigationService, IAlertService alertService)
    {
        _mediator = mediator;
        _navigationService = navigationService;
        _alertService = alertService;
    }

    protected bool ValidateAll()
    {
        ErrorMessages.Clear();
        ValidateAllProperties();

        if (HasErrors)
        {
            foreach (var error in GetErrors())
            {
                var property = error.MemberNames.First();
                var message = error.ErrorMessage;

                var added = ErrorMessages.TryAdd(property, message);
                if (!added)
                {
                    var oldMessage = ErrorMessages[property];
                    ErrorMessages[property] = $"{oldMessage}\n{message}";
                }
            }
        }

        OnPropertyChanged(nameof(ErrorMessages));
        return HasErrors;
    }

    protected void AddCustomError(string name, string message)
    {
        var added = ErrorMessages.TryAdd(name, message);
        if (!added)
        {
            var oldMessage = ErrorMessages[name];
            ErrorMessages[name] = $"{oldMessage}\n{message}";
        }
        OnPropertyChanged(nameof(ErrorMessages));
    }

    protected virtual void Cleanup()
    {
        ErrorMessages.Clear();
        OnPropertyChanged(nameof(ErrorMessages));
    }
}
