using FluentResults;
using Kris.Common.Models;
using MediatR;

namespace Kris.Client.Core.Requests;

public sealed class EditSessionUserCommand : IRequest<Result>
{
    public string UserName { get; set; }
    public MapPointSymbol UserSymbol { get; set; }
}
