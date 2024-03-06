using CommunityToolkit.Mvvm.ComponentModel;
using Kris.Client.Common.Constants;
using Kris.Client.Core.Services;
using MediatR;

namespace Kris.Client.ViewModels.Views;

public sealed partial class ChatViewModel : PageViewModelBase, IQueryAttributable
{
    [ObservableProperty]
    private Guid _conversationId;
    [ObservableProperty]
    private string _conversationName;

    public ChatViewModel(IMediator mediator, IRouterService navigationService, IMessageService messageService, IAlertService alertService)
        : base(mediator, navigationService, messageService, alertService)
    {
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        ConversationId = (Guid)query[QueryConstants.ContactsToChat.ConversationId];
        ConversationName = (string)query[QueryConstants.ContactsToChat.ConversationName];
    }
}
