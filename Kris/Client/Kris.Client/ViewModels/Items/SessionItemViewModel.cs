using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Kris.Client.Common.Enums;
using Kris.Client.Common.Events;
using Kris.Client.Core.Models;
using Kris.Common.Enums;
namespace Kris.Client.ViewModels.Items;

public sealed partial class SessionItemViewModel : ControllViewModelBase
{
    public event EventHandler<EntityIdEventArgs> SessionJoining;
    public event EventHandler<EntityIdEventArgs> SessionLeaving;
    public event EventHandler<EntityIdEventArgs> SessionEditing;

    [ObservableProperty]
    private Guid _id;
    [ObservableProperty]
    private string _name;
    [ObservableProperty]
    private DateTime _created;
    [ObservableProperty]
    private int _userCount;
    [ObservableProperty]
    private SessionItemType _itemType;
    [ObservableProperty]
    private bool _canEdit;

    public SessionItemViewModel(SessionListModel model, SessionItemType type, UserType? userType)
    {
        Id = model.Id;
        Name = model.Name;
        Created = TimeZoneInfo.ConvertTimeFromUtc(model.Created, TimeZoneInfo.Local);
        UserCount = model.UserCount;
        ItemType = type;
        CanEdit = type == SessionItemType.Current;
    }

    [RelayCommand]
    public void OnJoinClicked() => SessionJoining?.Invoke(this, new EntityIdEventArgs(Id));

    [RelayCommand]
    public void OnLeaveClicked() => SessionLeaving?.Invoke(this, new EntityIdEventArgs(Id));

    [RelayCommand]
    public void OnEditClicked() => SessionEditing?.Invoke(this, new EntityIdEventArgs(Id));
}
