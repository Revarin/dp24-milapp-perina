using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Kris.Client.ViewModels.Items;

public sealed partial class ImageItemViewModel : ControllViewModelBase
{
    public event EventHandler DeleteClicked;

    public string FilePath { get; set; }

    [ObservableProperty]
    private ImageSource _imageSource;
    [ObservableProperty]
    private bool _canDelete;

    public ImageItemViewModel(Stream imageStream, string filePath, bool canDelete)
    {
        ImageSource = ImageSource.FromStream(() => imageStream);
        FilePath = filePath;
        CanDelete = canDelete;
    }

    [RelayCommand]
    private void OnDeleteButtonClicked() => DeleteClicked?.Invoke(this, EventArgs.Empty);
}
