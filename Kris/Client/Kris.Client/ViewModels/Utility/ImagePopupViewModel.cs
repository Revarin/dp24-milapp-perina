using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Kris.Client.ViewModels.Utility;

public sealed partial class ImagePopupViewModel : ObservableObject
{
    public event EventHandler CancelClosing;

    [ObservableProperty]
    private ImageSource _imageSource;

    [RelayCommand]
    private void OnCancelButtonClicked() => CancelClosing?.Invoke(this, EventArgs.Empty);
}
