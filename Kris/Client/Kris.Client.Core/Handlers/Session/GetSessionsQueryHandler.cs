using FluentResults;
using Kris.Client.Core.Mappers;
using Kris.Client.Core.Models;
using Kris.Client.Core.Requests;
using Kris.Interface.Controllers;
using MediatR;

namespace Kris.Client.Core.Handlers.Session;

public sealed class GetSessionsQueryHandler : SessionHandler, IRequestHandler<GetSessionsQuery, Result<IEnumerable<SessionListModel>>>
{
    private readonly ISessionMapper _sessionMapper;

    public GetSessionsQueryHandler(ISessionMapper sessionMapper, ISessionController sessionClient)
        : base(sessionClient)
    {
        _sessionMapper = sessionMapper;
    }

    public async Task<Result<IEnumerable<SessionListModel>>> Handle(GetSessionsQuery request, CancellationToken cancellationToken)
    {
        var response = await _sessionClient.GetSessions(cancellationToken);

        if (!response.IsSuccess())
        {
            // TODO
            throw new Exception();
        }

        return Result.Ok(response.Values.Select(_sessionMapper.Map));
    }
}
