using FluentResults;
using Kris.Client.Data.Models;
using MediatR;

namespace Kris.Client.Core.Requests;

public sealed class UpdateConnectionSettingsCommand : IRequest<Result>
{
    public ConnectionSettingsEntity ConnectionSettings { get; set; }
}
