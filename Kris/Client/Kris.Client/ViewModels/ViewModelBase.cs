using Kris.Client.Core.Services;
using MediatR;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Kris.Client.ViewModels;

public abstract class ViewModelBase : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;

    protected readonly IMediator _mediator;
    protected readonly IRouterService _navigationService;
    protected readonly IAlertService _alertService;

    public ViewModelBase(IMediator mediator, IRouterService navigationService, IAlertService alertService)
    {
        _mediator = mediator;
        _navigationService = navigationService;
        _alertService = alertService;
    }

    // Source: https://stackoverflow.com/a/32800248
    protected bool SetPropertyValue<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
    {
        if (value == null ? field != null : !value.Equals(field))
        {
            field = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

            return true;
        }

        return false;
    }
}
