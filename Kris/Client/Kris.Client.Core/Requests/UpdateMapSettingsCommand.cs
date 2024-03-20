using FluentResults;
using Kris.Client.Data.Models;
using MediatR;

namespace Kris.Client.Core.Requests;

public sealed class UpdateMapSettingsCommand : IRequest<Result>
{
    public MapSettingsEntity MapSettings { get; set; }
    public string CustomMapTilesDatabasePath { get; set; }
}
