using Kris.Common.Models;

namespace Kris.Client.Core.Models;

public sealed class SessionDetailModel
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public DateTime Created { get; set; }
    public string UserName { get; set; }
    public MapPointSymbol UserSymbol { get; set; }
}
