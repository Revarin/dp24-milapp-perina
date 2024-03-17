using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Net.Mail;

namespace Kris.Client.ViewModels.Items;

public sealed partial class ImageItemViewModel : ControllViewModelBase
{
    public event EventHandler DeleteClicked;

    public Guid? Id { get; set; }
    public string FilePath { get; set; }

    [ObservableProperty]
    private ImageSource _imageSource;
    [ObservableProperty]
    private bool _canDelete;

    private string _base64Data;

    public ImageItemViewModel(string filePath, bool canDelete)
    {
        ImageSource = ImageSource.FromFile(filePath);
        FilePath = filePath;
        CanDelete = canDelete;
    }

    public ImageItemViewModel(string base64Data, Guid? id, string filePath, bool canDelete)
    {
        Id = id;
        FilePath = filePath;
        CanDelete = canDelete;

        _base64Data = base64Data;
        ImageSource = ImageSource.FromStream(() => new MemoryStream(Convert.FromBase64String(_base64Data)));
    }

    [RelayCommand]
    private void OnDeleteButtonClicked() => DeleteClicked?.Invoke(this, EventArgs.Empty);
}
