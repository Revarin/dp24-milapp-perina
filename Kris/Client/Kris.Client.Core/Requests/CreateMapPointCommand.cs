using FluentResults;
using Kris.Common.Enums;
using MediatR;

namespace Kris.Client.Core.Requests;

public sealed class CreateMapPointCommand : IRequest<Result<Guid>>
{
    public string Name { get; set; }
    public string? Description { get; set; }
    public Location Location { get; set; }
    public MapPointSymbolShape Shape { get; set; }
    public MapPointSymbolColor Color { get; set; }
    public MapPointSymbolSign Sign { get; set; }
    public List<string> Attachments { get; set; }
}
