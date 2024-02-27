using FluentResults;
using Kris.Interface.Requests;
using MediatR;

namespace Kris.Server.Core.Requests;

public sealed class StoreUserSettingsCommand : AuthentizedRequest, IRequest<Result>
{
    public required StoreUserSettingsRequest StoreUserSettings { get; set; }
}
