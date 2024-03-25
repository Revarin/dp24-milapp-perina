namespace Kris.Client.ViewModels.Popups;

public sealed partial class LoadingPopupViewModel : ControllViewModelBase
{
    public event EventHandler FinishClosing;

    public void Finish() => FinishClosing?.Invoke(this, EventArgs.Empty);
}
