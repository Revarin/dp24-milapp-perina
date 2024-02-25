﻿namespace Kris.Client.Common.Events;

public sealed class PasswordEventArgs : EventArgs
{
    public string Password { get; init; }

    public PasswordEventArgs(string password)
    {
        Password = password;
    }
}