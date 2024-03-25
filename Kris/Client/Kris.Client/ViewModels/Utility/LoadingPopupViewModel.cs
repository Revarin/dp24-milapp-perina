using CommunityToolkit.Mvvm.ComponentModel;

namespace Kris.Client.ViewModels.Utility;

public sealed partial class LoadingPopupViewModel : ObservableObject
{
    public event EventHandler CancelClosing;

    public void Close() => CancelClosing?.Invoke(this, EventArgs.Empty);
}
