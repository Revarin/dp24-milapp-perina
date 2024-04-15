using CommunityToolkit.Maui.Core;

namespace Kris.Client.Core.Services;

public interface IAlertService
{
    Task ShowToastAsync(string message, ToastDuration toastDuration = ToastDuration.Short, double textSize = 14);
    void ShowToast(string message, ToastDuration toastDuration = ToastDuration.Short, double textSize = 14);
    Task ShowNotificationAsync(int id, string title, string subtitle, string message, DateTime? NotifyTime = null, TimeSpan? RepeatInterval = null);
}
