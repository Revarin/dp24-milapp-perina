using Kris.Common.Enums;

namespace Kris.Interface.Requests;

public sealed class EditSessionUserRoleRequest : RequestBase
{
    public required Guid UserId { get; set; }
    public required UserType UserType { get; set; }
}
