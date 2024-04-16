using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Kris.Client.Common.Constants;
using Kris.Client.Common.Errors;
using Kris.Client.Common.Events;
using Kris.Client.Common.Options;
using Kris.Client.Connection.Hubs;
using Kris.Client.Connection.Hubs.Events;
using Kris.Client.Core.Models;
using Kris.Client.Core.Requests;
using Kris.Client.Core.Services;
using Kris.Client.Utility;
using Kris.Client.ViewModels.Items;
using Kris.Client.ViewModels.Utility;
using Kris.Common.Extensions;
using MediatR;
using Microsoft.Extensions.Options;
using System.Collections.ObjectModel;

namespace Kris.Client.ViewModels.Views;

public sealed partial class ChatViewModel : PageViewModelBase, IQueryAttributable
{
    private readonly SettingsOptions _settingsOptions;
    private readonly IMessageReceiver _messageReceiver;

    [ObservableProperty]
    private IViewRequest<int> _scrollTo = new Client.Utility.ScrollToRequest();

    [ObservableProperty]
    private Guid _conversationId;
    [ObservableProperty]
    private string _conversationName;
    [ObservableProperty]
    private ObservableCollection<MessageItemViewModel> _messages = new ObservableCollection<MessageItemViewModel>();
    [ObservableProperty]
    private int _messageThreshold = -1;

    [ObservableProperty]
    private string _messageBody;

    public ChatViewModel(IOptions<SettingsOptions> options, IMessageReceiver messageReceiver,
        IMediator mediator, IRouterService navigationService, IMessageService messageService, IPopupService popupService, IAlertService alertService)
        : base(mediator, navigationService, messageService, popupService, alertService)
    {
        _settingsOptions = options.Value;
        _messageReceiver = messageReceiver;
    }

    // HANDLERS
    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        ConversationId = (Guid)query[QueryConstants.ContactsToChat.ConversationId];
        ConversationName = (string)query[QueryConstants.ContactsToChat.ConversationName];
    }

    [RelayCommand]
    private async Task OnAppearing()
    {
        _messageReceiver.MessageReceived += OnMessageReceived;
        await LoadMessagesAsync(0);
        await Task.Delay(1000).ContinueWith(_ =>
        {
            if (Messages.Count == _settingsOptions.ChatMessagesPageSize)
            {
                MessageThreshold = 5;
            }
        });
    }
    [RelayCommand]
    private async Task OnBackButtonClicked() => await GoToContactsAsync();
    [RelayCommand]
    private async Task OnSendButtonClicked() => await SendMessageAsync();
    [RelayCommand]
    private async Task OnDeleteButtonClicked() => await DeleteConversationAsync();
    [RelayCommand]
    private async Task OnCollectionItemsThresholdReached()
    {
        var lastMessage = Messages.Count - 1;
        await LoadMessagesAsync(Messages.Count / _settingsOptions.ChatMessagesPageSize);
        ScrollTo.Execute(lastMessage);
    }

    // CORE
    private async Task LoadMessagesAsync(int page)
    {
        var ct = new CancellationToken();
        var query = new GetMessagesQuery { ConversationId = ConversationId, Page = page };
        var result = await MediatorSendAsync(query, ct);

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
                await GoToContactsAsync();
            }
            else if (result.HasError<ConnectionError>())
            {
                await _alertService.ShowToastAsync("No connection to server");
                await GoToContactsAsync();
            }
            else
            {
                await _alertService.ShowToastAsync(result.Errors.FirstMessage());
            }
        }
        else
        {
            Messages = Messages.Concat(result.Value
                .OrderByDescending(m => m.TimeStamp)
                .Select(m => new MessageItemViewModel(m)))
                .ToObservableCollection();

            if (result.Value.Count() < _settingsOptions.ChatMessagesPageSize)
            {
                MessageThreshold = -1;
            }
        }
    }

    private async Task SendMessageAsync()
    {
        var ct = new CancellationToken();
        var command = new SendMessageCommand { ConversationId = ConversationId, Body = MessageBody };
        var result = await MediatorSendAsync(command, ct);

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
                await GoToContactsAsync();
            }
            else if (result.HasError<ConnectionError>())
            {
                await _alertService.ShowToastAsync("No connection to server");
            }
            else
            {
                await _alertService.ShowToastAsync(result.Errors.FirstMessage());
            }
        }
        else
        {
            MessageBody = string.Empty;
        }
    }

    private async Task DeleteConversationAsync()
    {
        var confirmation = await _popupService.ShowPopupAsync<ConfirmationPopupViewModel>(vm => vm.Message = "Delete this conversation?") as ConfirmationEventArgs;
        if (confirmation == null || !confirmation.IsConfirmed) return;

        var ct = new CancellationToken();
        var command = new DeleteConversationCommand { ConversationId = ConversationId };
        var result = await MediatorSendLoadingAsync(command, ct);

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
                await GoToContactsAsync();
            }
            else if (result.HasError<ForbiddenError>())
            {
                await _alertService.ShowToastAsync("Cannot delete conversation with active users");
            }
            else if (result.HasError<ConnectionError>())
            {
                await _alertService.ShowToastAsync("No connection to server");
            }
            else
            {
                await _alertService.ShowToastAsync(result.Errors.FirstMessage());
            }
        }
        else
        {
            await _alertService.ShowToastAsync("Conversation deleted");
            await GoToContactsAsync();
        }
    }

    private void OnMessageReceived(object sender, MessageReceivedEventArgs e)
    {
        var message = new MessageModel
        {
            Id = e.Id,
            SenderId = e.SenderId,
            SenderName = e.SenderName,
            Body = e.Body,
            TimeStamp = e.TimeStamp
        };
        Messages.Insert(0, new MessageItemViewModel(message));
        ScrollTo.Execute(0);
    }

    // MISC
    protected override Task OnMapButtonClicked()
    {
        _messageReceiver.MessageReceived -= OnMessageReceived;
        return base.OnMapButtonClicked();
    }

    protected override Task OnMenuButtonClicked()
    {
        _messageReceiver.MessageReceived -= OnMessageReceived;
        return base.OnMenuButtonClicked();
    }

    protected override Task LogoutUser()
    {
        _messageReceiver.MessageReceived -= OnMessageReceived;
        return base.LogoutUser();
    }

    private async Task GoToContactsAsync()
    {
        _messageReceiver.MessageReceived -= OnMessageReceived;
        await _navigationService.GoBackAsync();
    }
}
