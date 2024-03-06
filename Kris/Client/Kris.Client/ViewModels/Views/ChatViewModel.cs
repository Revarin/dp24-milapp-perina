using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Kris.Client.Common.Constants;
using Kris.Client.Common.Errors;
using Kris.Client.Common.Utility;
using Kris.Client.Core.Requests;
using Kris.Client.Core.Services;
using Kris.Client.ViewModels.Items;
using Kris.Common.Extensions;
using MediatR;
using System.Collections.ObjectModel;

namespace Kris.Client.ViewModels.Views;

public sealed partial class ChatViewModel : PageViewModelBase, IQueryAttributable
{
    [ObservableProperty]
    private Guid _conversationId;
    [ObservableProperty]
    private string _conversationName;
    [ObservableProperty]
    private ObservableCollection<MessageItemViewModel> _messages;

    public ChatViewModel(IMediator mediator, IRouterService navigationService, IMessageService messageService, IAlertService alertService)
        : base(mediator, navigationService, messageService, alertService)
    {
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        ConversationId = (Guid)query[QueryConstants.ContactsToChat.ConversationId];
        ConversationName = (string)query[QueryConstants.ContactsToChat.ConversationName];
    }

    [RelayCommand]
    private async Task OnAppearing() => await LoadMessages();

    // Implementation
    private async Task LoadMessages()
    {
        var ct = new CancellationToken();
        var query = new GetMessagesQuery { ConversationId = ConversationId, Page = 0 };
        var result = await _mediator.Send(query, ct);

        if (result.IsFailed)
        {
            if (result.HasError<UnauthorizedError>())
            {
                await _alertService.ShowToastAsync("Login expired");
                await LogoutUser();
            }
            else if (result.HasError<EntityNotFoundError>())
            {
                await _alertService.ShowToastAsync("Conversation does not exists");
                await _navigationService.GoToAsync("..", RouterNavigationType.PushUpward);
            }
            else
            {
                await _alertService.ShowToastAsync(result.Errors.FirstMessage());
            }
        }
        else
        {
            Messages = result.Value.Select(m => new MessageItemViewModel(m)).ToObservableCollection();
        }
    }
}
