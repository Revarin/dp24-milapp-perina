using CommunityToolkit.Maui.Core;

namespace Kris.Client.Core.Services;

public interface IAlertService
{
    Task ShowToastAsync(string message, ToastDuration toastDuration = ToastDuration.Short, double textSize = 14);
    void ShowToast(string message, ToastDuration toastDuration = ToastDuration.Short, double textSize = 14);
}
