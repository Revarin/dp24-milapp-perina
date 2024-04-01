using FluentResults;
using Kris.Common.Enums;
using MediatR;

namespace Kris.Client.Core.Requests;

public sealed class EditSessionUserRoleCommand : IRequest<Result>
{
    public Guid UserId { get; set; }
    public UserType NewRole { get; set; }
}
