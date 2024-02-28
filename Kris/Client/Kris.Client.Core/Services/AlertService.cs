using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;

namespace Kris.Client.Core.Services;

public sealed class AlertService : IAlertService
{
    public void ShowToast(string message, ToastDuration toastDuration = ToastDuration.Short, double textSize = 14)
    {
        Application.Current.MainPage.Dispatcher.Dispatch(async () =>
        {
            await ShowToastAsync(message, toastDuration, textSize);
        });
    }

    // Can only be used in UI thread, use ShowToast instead
    public Task ShowToastAsync(string message, ToastDuration toastDuration = ToastDuration.Short, double textSize = 14)
    {
        var toast = Toast.Make(message, toastDuration, textSize);
        return toast.Show();
    }
}
