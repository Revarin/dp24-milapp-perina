using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Kris.Client.Core.Models;

namespace Kris.Client.ViewModels.Items;

public sealed partial class ConversationItemViewModel : ControllViewModelBase
{
    public event EventHandler ContactClicked;

    [ObservableProperty]
    private Guid _id;
    [ObservableProperty]
    private string _name;
    [ObservableProperty]
    private int _messageCount;
    [ObservableProperty]
    private DateTime? _lastMessage;

    public ConversationItemViewModel(ConversationModel model)
    {
        Id = model.Id;
        Name = model.Name;
        MessageCount = model.MessageCount;
        LastMessage = model.LastMessage.HasValue ? TimeZoneInfo.ConvertTimeFromUtc(model.LastMessage.Value, TimeZoneInfo.Local) : null;
    }

    [RelayCommand]
    private void OnClicked() => ContactClicked?.Invoke(this, EventArgs.Empty);
}
