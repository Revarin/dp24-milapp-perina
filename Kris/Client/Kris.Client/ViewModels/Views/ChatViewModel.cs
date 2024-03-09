using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Kris.Client.Common.Constants;
using Kris.Client.Common.Errors;
using Kris.Client.Connection.Hubs;
using Kris.Client.Connection.Hubs.Events;
using Kris.Client.Core.Models;
using Kris.Client.Core.Requests;
using Kris.Client.Core.Services;
using Kris.Client.ViewModels.Items;
using Kris.Common.Extensions;
using MediatR;
using System.Collections.ObjectModel;

namespace Kris.Client.ViewModels.Views;

public sealed partial class ChatViewModel : PageViewModelBase, IQueryAttributable
{
    private readonly IMessageReceiver _messageReceiver;

    [ObservableProperty]
    private Guid _conversationId;
    [ObservableProperty]
    private string _conversationName;
    [ObservableProperty]
    private ObservableCollection<MessageItemViewModel> _messages;

    [ObservableProperty]
    private string _messageBody;

    public ChatViewModel(IMessageReceiver messageReceiver,
        IMediator mediator, IRouterService navigationService, IMessageService messageService, IAlertService alertService)
        : base(mediator, navigationService, messageService, alertService)
    {
        _messageReceiver = messageReceiver;
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        ConversationId = (Guid)query[QueryConstants.ContactsToChat.ConversationId];
        ConversationName = (string)query[QueryConstants.ContactsToChat.ConversationName];
    }

    [RelayCommand]
    private async Task OnAppearing()
    {
        _messageReceiver.MessageReceived += OnMessageReceived;
        await LoadMessagesAsync();
    }

    [RelayCommand]
    private async Task OnBackButtonPressed()
    {
        _messageReceiver.MessageReceived -= OnMessageReceived;
        await _navigationService.GoBackAsync();
    }

    [RelayCommand]
    private async Task OnSendPressed() => await SendMessageAsync();

    // CORE
    private async Task LoadMessagesAsync()
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
                _messageReceiver.MessageReceived -= OnMessageReceived;
                await _navigationService.GoBackAsync();
            }
            else
            {
                await _alertService.ShowToastAsync(result.Errors.FirstMessage());
            }
        }
        else
        {
            Messages = result.Value
                .OrderBy(m => m.TimeStamp)
                .Select(m => new MessageItemViewModel(m))
                .ToObservableCollection();
        }
    }

    private async Task SendMessageAsync()
    {
        var ct = new CancellationToken();
        var command = new SendMessageCommand { ConversationId = ConversationId, Body = MessageBody };
        var result = await _mediator.Send(command, ct);

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
                _messageReceiver.MessageReceived -= OnMessageReceived;
                await _navigationService.GoBackAsync();
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
        Messages.Add(new MessageItemViewModel(message));
    }

    // MISC
    protected override Task GoToMap()
    {
        _messageReceiver.MessageReceived -= OnMessageReceived;
        return base.GoToMap();
    }

    protected override Task GoToMenu()
    {
        _messageReceiver.MessageReceived -= OnMessageReceived;
        return base.GoToMenu();
    }

    protected override Task LogoutUser()
    {
        _messageReceiver.MessageReceived -= OnMessageReceived;
        return base.LogoutUser();
    }
}
