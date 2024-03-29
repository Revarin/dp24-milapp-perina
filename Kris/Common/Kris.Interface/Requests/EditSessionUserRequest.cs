using Kris.Common.Models;

namespace Kris.Interface.Requests;

public sealed class EditSessionUserRequest : RequestBase
{
    public required string Nickname { get; set; }
    public required MapPointSymbol Symbol { get; set; }
}
