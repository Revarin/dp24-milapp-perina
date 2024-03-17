using FluentResults;
using Kris.Server.Common.Errors;
using Kris.Server.Common.Exceptions;
using Kris.Server.Core.Mappers;
using Kris.Server.Core.Requests;
using Kris.Server.Core.Services;
using Kris.Server.Data.Models;
using Kris.Server.Data.Repositories;
using MediatR;

namespace Kris.Server.Core.Handlers.MapObject;

public sealed class AddMapPointCommandHandler : MapObjectHandler, IRequestHandler<AddMapPointCommand, Result<Guid>>
{
    private readonly IMapPointRepository _mapPointRepository;
    private readonly IAttachmentMapper _attachmentMapper;

    public AddMapPointCommandHandler(IMapPointRepository mapPointRepository, IAttachmentMapper attachmentMapper,
        IAuthorizationService authorizationService)
        : base(authorizationService)
    {
        _mapPointRepository = mapPointRepository;
        _attachmentMapper = attachmentMapper;
    }

    public async Task<Result<Guid>> Handle(AddMapPointCommand request, CancellationToken cancellationToken)
    {
        var user = request.User;
        if (!user.SessionId.HasValue) return Result.Fail(new UnauthorizedError(user.Login, user.SessionName, user.UserType));

        var authResult = await _authorizationService.AuthorizeAsync(user, cancellationToken);
        if (!authResult.IsAuthorized) return Result.Fail(new UnauthorizedError(user.Login, user.SessionName, user.UserType));

        var mapPoint = new MapPointEntity
        {
            Name = request.AddMapPoint.Name,
            Type = request.AddMapPoint.Type,
            Description = request.AddMapPoint.Description,
            Position = request.AddMapPoint.Position,
            Symbol = request.AddMapPoint.Symbol,
            Created = DateTime.UtcNow,
            SessionUserId = authResult.SessionUserId,
        };
        mapPoint.Attachments.AddRange(request.AddMapPoint.Attachments
            .Select(attachment => _attachmentMapper.MapPointAttachment(attachment, mapPoint.Id)));
        var entity = await _mapPointRepository.InsertAsync(mapPoint, cancellationToken);
        if (entity == null) throw new DatabaseException("Failed to insert map point");

        return Result.Ok(entity.Id);
    }
}
