﻿namespace Kris.Interface.Models;

public class SessionListModel
{
    public required Guid Id { get; set; }
    public required string Name { get; set; }
    public required DateTime Created { get; set; }
    public required int UserCount { get; set; }
}
