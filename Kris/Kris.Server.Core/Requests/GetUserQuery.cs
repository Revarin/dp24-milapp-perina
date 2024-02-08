using FluentResults;
using Kris.Server.Core.Models;
using MediatR;

namespace Kris.Server.Core.Requests;

public class GetUserQuery : IRequest<Result<UserModel>>
{
    public required Guid Id { get; set; }
}
