using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using Plugin.LocalNotification;

namespace Kris.Client.Core.Services;

public sealed class AlertService : IAlertService
{
    public async Task ShowNotificationAsync(int id, string title, string subtitle, string message, DateTime? NotifyTime = null, TimeSpan? RepeatInterval = null)
    {
        var request = new NotificationRequest
        {
            NotificationId = id,
            Title = title,
            Subtitle = subtitle,
            Description = message,
            CategoryType = NotificationCategoryType.Alarm,
            Schedule = new NotificationRequestSchedule
            {
                NotifyTime = NotifyTime,
                NotifyRepeatInterval = RepeatInterval
            },
            Android = new Plugin.LocalNotification.AndroidOption.AndroidOptions
            {
                AutoCancel = true,
                LaunchAppWhenTapped = false,
                ChannelId = "krisMessageChannel"
            },
        };

        await LocalNotificationCenter.Current.Show(request);
    }

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
