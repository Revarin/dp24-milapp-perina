namespace Kris.Client.Common.Events;

public sealed class ConfirmationEventArgs : EventArgs
{
    public bool IsConfirmed { get; init; }

    public ConfirmationEventArgs(bool isConfirmed)
    {
        IsConfirmed = isConfirmed;
    }
}
