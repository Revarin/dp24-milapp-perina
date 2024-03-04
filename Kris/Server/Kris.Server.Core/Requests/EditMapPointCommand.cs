using FluentResults;
using Kris.Interface.Requests;
using MediatR;

namespace Kris.Server.Core.Requests;

public sealed class EditMapPointCommand : AuthentizedRequest, IRequest<Result>
{
    public required EditMapPointRequest EditMapPoint { get; set; }
}
