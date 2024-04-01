using FluentResults;
using Kris.Interface.Requests;
using MediatR;

namespace Kris.Server.Core.Requests;

public sealed class EditSessionUserRoleCommand : AuthentizedRequest, IRequest<Result>
{
    public required EditSessionUserRoleRequest EditSessionUserRole { get; set; }
}
