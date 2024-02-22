namespace Kris.Client.Events;

public sealed class PasswordEventArgs : EventArgs
{
    public string Password { get; private set; }

    public PasswordEventArgs(string password)
    {
        Password = password;
    }
}
