using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Kris.Client.Core.Models;
using Kris.Common.Enums;

namespace Kris.Client.ViewModels.Items;

public sealed partial class SessionUserItemViewModel : ControllViewModelBase
{
    public event EventHandler UserKicking;
    public event EventHandler UserRoleChanging;

    public Guid Id { get; set; }

    [ObservableProperty]
    private string _login;
    [ObservableProperty]
    private string _nickname;
    [ObservableProperty]
    private UserType _userType;

    public SessionUserItemViewModel(SessionUserModel model)
    {
        Id = model.Id;
        Login = model.Login;
        Nickname = model.Nickname;
        UserType = model.UserType;
    }

    [RelayCommand]
    private void OnUserAdminToggled(ToggledEventArgs e)
    {
        UserType = e.Value ? UserType.Admin : UserType.Basic;
        UserRoleChanging?.Invoke(this, EventArgs.Empty);
    }

    [RelayCommand]
    private void OnKickUserClicked() => UserKicking?.Invoke(this, EventArgs.Empty);

    public void RevertUserType(UserType revertedType)
    {
        if (revertedType == UserType.Basic) UserType = UserType.Admin;
        else if (revertedType == UserType.Admin) UserType = UserType.Basic;
    }
}
