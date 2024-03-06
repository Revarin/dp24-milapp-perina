using CommunityToolkit.Mvvm.ComponentModel;
using Kris.Client.Core.Models;

namespace Kris.Client.ViewModels.Items;

public sealed partial class ConversationItemViewModel : ControllViewModelBase
{
    [ObservableProperty]
    private Guid _id;
    [ObservableProperty]
    private string _name;
    [ObservableProperty]
    private int _messageCount;
    [ObservableProperty]
    private DateTime? _lastMessage;

    public ConversationItemViewModel(ConversationListModel model)
    {
        Id = model.Id;
        Name = model.Name;
        MessageCount = model.MessageCount;
        LastMessage = model.LastMessage;
    }
}
