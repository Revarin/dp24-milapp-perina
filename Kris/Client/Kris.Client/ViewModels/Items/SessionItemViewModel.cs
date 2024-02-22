using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Kris.Client.Common.Enums;
using Kris.Client.Core.Models;
using Kris.Client.Events;
namespace Kris.Client.ViewModels.Items;

public sealed partial class SessionItemViewModel : ControllViewModelBase
{
    public event EventHandler<EntityIdEventArgs> SessionJoining;
    public event EventHandler<EntityIdEventArgs> SessionLeaving;

    [ObservableProperty]
    private Guid _id;
    [ObservableProperty]
    private string _name;
    [ObservableProperty]
    private DateTime _created;
    [ObservableProperty]
    private int _userCount;
    [ObservableProperty]
    private bool _isJoined;

    public SessionItemViewModel(SessionListModel model, SessionItemType type)
    {
        Id = model.Id;
        Name = model.Name;
        Created = model.Created;
        UserCount = model.UserCount;

        if (type == SessionItemType.Current || type == SessionItemType.Joined)
        {
            IsJoined = true;
        }
        else
        {
            IsJoined = false;
        }
    }

    [RelayCommand]
    public void OnJoinClicked() => SessionJoining?.Invoke(this, new EntityIdEventArgs(Id));

    [RelayCommand]
    public void OnLeaveClicked() => SessionLeaving?.Invoke(this, new EntityIdEventArgs(Id));
}
