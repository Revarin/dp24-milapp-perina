namespace Kris.Client.Core
{
    public interface IAlertService
    {
        Task ShowAlertAsync(string title, string message, string cancel = "OK");
        void ShowAlert(string title, string message, string cancel = "OK");
        Task<bool> ShowConfirmationAsync(string title, string message, string accept = "Yes", string cancel = "No");
        void ShowConfirmation(string title, string message, Action<bool> callback, string accept = "Yes", string cancel = "No");
        Task<string> ShowPromptAsync(string title, string message, string accept = "OK", string cancel = "Cancel", string placeholder = null, int maxLenght = -1, string initialValue = null);
        public void ShowPrompt(string title, string message, Action<string> callback, string accept = "OK", string cancel = "Cancel", string placeholder = null, int maxLenght = -1, string initialValue = null);
    }
}
