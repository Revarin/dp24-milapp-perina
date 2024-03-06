using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Kris.Client.Common.Errors;
using Kris.Client.Core.Requests;
using Kris.Client.Core.Services;
using Kris.Client.ViewModels.Items;
using Kris.Common.Extensions;
using MediatR;
using System.Collections.ObjectModel;

namespace Kris.Client.ViewModels.Views;

public sealed partial class ContactsViewModel : PageViewModelBase
{
    [ObservableProperty]
    private ObservableCollection<ConversationItemViewModel> _contacts;

    public ContactsViewModel(IMediator mediator, IRouterService navigationService, IMessageService messageService, IAlertService alertService)
        : base(mediator, navigationService, messageService, alertService)
    {
    }

    [RelayCommand]
    private async Task OnAppearing() => await LoadConversations();

    // Implementations
    private async Task LoadConversations()
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
            Contacts = result.Value.Select(c => new ConversationItemViewModel(c)).ToObservableCollection();
        }
    }
}
