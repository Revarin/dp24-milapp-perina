using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Kris.Client.Common.Constants;
using Kris.Client.Common.Errors;
using Kris.Client.Common.Utility;
using Kris.Client.Core.Requests;
using Kris.Client.Core.Services;
using Kris.Client.ViewModels.Items;
using Kris.Client.Views;
using Kris.Common.Extensions;
using MediatR;
using System.Collections.ObjectModel;

namespace Kris.Client.ViewModels.Views;

public sealed partial class ContactsViewModel : PageViewModelBase
{
    [ObservableProperty]
    private ObservableCollection<ConversationItemViewModel> _specialContacts;
    [ObservableProperty]
    private ObservableCollection<ConversationItemViewModel> _directContacts;

    public ContactsViewModel(IMediator mediator, IRouterService navigationService, IMessageService messageService, IAlertService alertService)
        : base(mediator, navigationService, messageService, alertService)
    {
    }

    // HANDLERS
    [RelayCommand]
    private async Task OnAppearing() => await LoadConversationsAsync();

    // CORE
    private async Task LoadConversationsAsync()
    {
        var ct = new CancellationToken();
        var query = new GetConversationsQuery();
        var result = await _mediator.Send(query, ct);

        if (result.IsFailed)
        {
            if (result.HasError<UnauthorizedError>())
            {
                await _alertService.ShowToastAsync("Login expired");
                await LogoutUser();
            }
            else
            {
                await _alertService.ShowToastAsync(result.Errors.FirstMessage());
            }
        }
        else
        {
            SpecialContacts = result.Value.SpecialConversations.Select(c => new ConversationItemViewModel(c)).ToObservableCollection();
            foreach (var c in SpecialContacts)
            {
                c.ContactClicked += OnContactClicked;
            }
            DirectContacts = result.Value.DirectConversations.Select(c => new ConversationItemViewModel(c)).ToObservableCollection();
            foreach (var c in DirectContacts)
            {
                c.ContactClicked += OnContactClicked;
            }
        }
    }

    private async void OnContactClicked(object sender, EventArgs e)
    {
        if (sender is ConversationItemViewModel contact)
        {
            var parameters = new Dictionary<string, object>
            {
                { QueryConstants.ContactsToChat.ConversationId, contact.Id },
                { QueryConstants.ContactsToChat.ConversationName, contact.Name }
            };
            await _navigationService.GoToAsync(nameof(ChatView), parameters, RouterNavigationType.PushUpward);
        }
    }
}
