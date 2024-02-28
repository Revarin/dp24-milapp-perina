using CommunityToolkit.Mvvm.ComponentModel;
using MediatR;

namespace Kris.Client.ViewModels;

public abstract partial class FormViewModelBase : ObservableValidator
{
    protected readonly IMediator _mediator;

    public Task InitializationWork { get; private set; }

    [ObservableProperty]
    protected bool _isLoading;
    [ObservableProperty]
    protected Dictionary<string, string> errorMessages = new Dictionary<string, string>();

    protected FormViewModelBase(IMediator mediator)
    {
        _mediator = mediator;
    }

    // Source: https://www.reddit.com/r/dotnetMAUI/comments/16b2uy7/async_data_loading_on_page_load/
    public async void Init()
    {
        try
        {
            IsLoading = true;
            InitializationWork = InitAsync();
            await InitializationWork;
        }
        catch
        {
            throw;
        }
        finally
        {
            IsLoading = false;
        }
    }

    protected virtual Task InitAsync()
    {
        return Task.CompletedTask;
    }

    protected virtual void Cleanup()
    {
        ErrorMessages.Clear();
        OnPropertyChanged(nameof(ErrorMessages));
    }

    protected new bool ValidateAllProperties()
    {
        ErrorMessages.Clear();
        base.ValidateAllProperties();

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
}
