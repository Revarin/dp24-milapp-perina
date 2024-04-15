using CommunityToolkit.Mvvm.ComponentModel;
using Kris.Client.Core.Models;

namespace Kris.Client.ViewModels.Items;

public sealed partial class MessageItemViewModel : ControllViewModelBase
{
    [ObservableProperty]
    private Guid _id;
    [ObservableProperty]
    private Guid? _senderId;
    [ObservableProperty]
    private string _senderName;
    [ObservableProperty]
    private DateTime _sent;
    [ObservableProperty]
    private string _body;

    public MessageItemViewModel(MessageModel model)
    {
        Id = model.Id;
        SenderId = model.SenderId;
        SenderName = string.IsNullOrEmpty(model.SenderName) ? "[Unknown]" : model.SenderName;
        Sent = TimeZoneInfo.ConvertTimeFromUtc(model.TimeStamp, TimeZoneInfo.Local);
        Body = model.Body;
    }
}
