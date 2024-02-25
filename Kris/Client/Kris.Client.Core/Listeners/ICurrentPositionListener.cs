﻿using Kris.Client.Common.Events;

namespace Kris.Client.Core.Listeners;

public interface ICurrentPositionListener : IBackgroundListener
{
    event EventHandler<LocationEventArgs> PositionChanged;
}