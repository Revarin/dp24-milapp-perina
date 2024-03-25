using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Kris.Client.ViewModels.Popups;
using Kris.Client.ViewModels.Utility;

namespace Kris.Client.ViewModels.Items;

public sealed partial class ImageItemViewModel : ControllViewModelBase
{
    private readonly IPopupService _popupService;

    public event EventHandler DeleteClicked;

    public Guid? Id { get; set; }
    public string FilePath { get; set; }

    [ObservableProperty]
    private ImageSource _imageSource;
    [ObservableProperty]
    private bool _canDelete;

    private string _base64Data;

    public ImageItemViewModel(IPopupService popupService, string filePath, bool canDelete)
    {
        _popupService = popupService;

        ImageSource = ImageSource.FromFile(filePath);
        FilePath = filePath;
        CanDelete = canDelete;
    }

    public ImageItemViewModel(IPopupService popupService, string base64Data, Guid? id, string filePath, bool canDelete)
    {
        _popupService = popupService;

        Id = id;
        FilePath = filePath;
        CanDelete = canDelete;

        _base64Data = base64Data;
        ImageSource = ImageSource.FromStream(() => new MemoryStream(Convert.FromBase64String(_base64Data)));
    }

    [RelayCommand]
    private async Task OnImageClicked()
    {
        await _popupService.ShowPopupAsync<ImagePopupViewModel>(vm =>
        {
            vm.ImageSource = ImageSource;
        });
    }

    [RelayCommand]
    private void OnDeleteButtonClicked() => DeleteClicked?.Invoke(this, EventArgs.Empty);
}
