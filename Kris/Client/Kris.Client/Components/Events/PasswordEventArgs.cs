namespace Kris.Client.Components.Events;

public sealed class PasswordEventArgs : EventArgs
{
    public string Password { get; init; }

    public PasswordEventArgs(string password)
    {
        Password = password;
    }
}
