using Kris.Common.Models;

namespace Kris.Interface.Models;

public sealed class SessionDetailModel
{
    public required Guid Id { get; set; }
    public required string Name { get; set; }
    public required DateTime Created { get; set; }

    public required string UserName { get; set; }
    public required MapPointSymbol UserSymbol { get; set; }

    public IEnumerable<SessionUserModel> Users { get; set; } = new List<SessionUserModel>();
}
