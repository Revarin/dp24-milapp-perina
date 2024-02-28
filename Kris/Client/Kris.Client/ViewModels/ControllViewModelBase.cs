using CommunityToolkit.Mvvm.ComponentModel;

namespace Kris.Client.ViewModels;

public abstract partial class ControllViewModelBase : ObservableObject
{
    public Task InitializationWork { get; private set; }

    [ObservableProperty]
    protected bool _isLoading;

    protected ControllViewModelBase()
    {
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
}
