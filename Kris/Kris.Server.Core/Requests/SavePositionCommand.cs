using FluentResults;
using Kris.Interface.Requests;
using MediatR;

namespace Kris.Server.Core.Requests;

public class SavePositionCommand : AuthentizedRequest, IRequest<Result>
{
    public required SavePositionRequest SavePosition { get; set; }
}
