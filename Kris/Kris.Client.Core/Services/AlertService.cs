using CommunityToolkit.Maui.Alerts;

namespace Kris.Client.Core
{
    // Source: https://stackoverflow.com/a/72439742
    public class AlertService : IAlertService
    {
        public void ShowAlert(string title, string message, string cancel = "OK")
        {
            Application.Current.MainPage.Dispatcher.Dispatch(async () => 
                await ShowAlertAsync(title, message, cancel));
        }

        public Task ShowAlertAsync(string title, string message, string cancel = "OK")
        {
            return Application.Current.MainPage.DisplayAlert(title, message, cancel);
        }

        public void ShowConfirmation(string title, string message, Action<bool> callback, string accept = "Yes", string cancel = "No")
        {
            Application.Current.MainPage.Dispatcher.Dispatch(async () =>
            {
                var result = await ShowConfirmationAsync(title, message, accept, cancel);
                callback(result);
            });
        }

        public Task<bool> ShowConfirmationAsync(string title, string message, string accept = "Yes", string cancel = "No")
        {
            return Application.Current.MainPage.DisplayAlert(title, message, accept, cancel);
        }

        public void ShowPrompt(string title, string message, Action<string> callback, string accept = "OK", string cancel = "Cancel", string placeholder = null, int maxLenght = -1, string initialValue = null)
        {
            Application.Current.MainPage.Dispatcher.Dispatch(async () =>
            {
                var result = await ShowPromptAsync(title, message, accept, cancel, placeholder, maxLenght, initialValue);
                callback(result);
            });
        }

        public Task<string> ShowPromptAsync(string title, string message, string accept = "OK", string cancel = "Cancel", string placeholder = null, int maxLenght = -1, string initialValue = null)
        {
            return Application.Current.MainPage.DisplayPromptAsync(title, message, accept, cancel, placeholder, maxLenght, null, initialValue);
        }

        public void ShowToast(string message)
        {
            Application.Current.MainPage.Dispatcher.Dispatch(async () =>
            {
                var toast = Toast.Make(message);
                await toast.Show();
            });
        }

        public Task ShowToastAsync(string message)
        {
            throw new NotImplementedException();
        }
    }
}
