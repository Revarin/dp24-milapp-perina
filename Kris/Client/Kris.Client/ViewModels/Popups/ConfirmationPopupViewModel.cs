using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Kris.Client.Common.Events;

namespace Kris.Client.ViewModels.Popups;

public sealed partial class ConfirmationPopupViewModel : ControllViewModelBase
{
    public event EventHandler<ConfirmationEventArgs> YesClosing;
    public event EventHandler<ConfirmationEventArgs> NoClosing;

    [ObservableProperty]
    private string _message;

    [RelayCommand]
    private void OnYesButtonClicked() => YesClosing?.Invoke(this, new ConfirmationEventArgs(true));
    [RelayCommand]
    private void OnNoButtonClicked() => NoClosing?.Invoke(this, new ConfirmationEventArgs(false));
}
