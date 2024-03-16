using FluentResults;
using Kris.Common.Enums;
using Kris.Interface.Enums;
using Kris.Server.Common.Errors;
using Kris.Server.Core.Mappers;
using Kris.Server.Core.Requests;
using Kris.Server.Core.Services;
using Kris.Server.Data.Repositories;
using MediatR;

namespace Kris.Server.Core.Handlers.MapObject;

public sealed class EditMapPointCommandHandler : MapObjectHandler, IRequestHandler<EditMapPointCommand, Result>
{
    private readonly IMapPointRepository _mapPointRepository;
    private readonly IAttachmentMapper _attachmentMapper;

    public EditMapPointCommandHandler(IMapPointRepository mapPointRepository, IAttachmentMapper attachmentMapper,
        IAuthorizationService authorizationService)
        : base(authorizationService)
    {
        _mapPointRepository = mapPointRepository;
        _attachmentMapper = attachmentMapper;
    }

    public async Task<Result> Handle(EditMapPointCommand request, CancellationToken cancellationToken)
    {
        var user = request.User;
        var mapPoint = await _mapPointRepository.GetWithAllAsync(request.EditMapPoint.Id, cancellationToken);
        if (mapPoint == null) return Result.Fail(new EntityNotFoundError("MapPoint", request.EditMapPoint.Id));

        var minRole = user.UserId == mapPoint.SessionUser!.UserId ? UserType.Basic : UserType.Admin;
        var authResult = await _authorizationService.AuthorizeAsync(user, minRole, cancellationToken);
        if (!authResult.IsAuthorized) return Result.Fail(new UnauthorizedError(user.Login, user.SessionName, user.UserType));

        mapPoint.Name = request.EditMapPoint.Name;
        mapPoint.Description = request.EditMapPoint.Description;
        mapPoint.Type = request.EditMapPoint.Type;
        mapPoint.Position = request.EditMapPoint.Position;
        mapPoint.Symbol = request.EditMapPoint.Symbol;
        mapPoint.Created = DateTime.UtcNow;

        mapPoint.Attachments.RemoveAll(attachment => request.EditMapPoint.DeletedAttachments
            .Any(deletedId => deletedId == attachment.Id));
        mapPoint.Attachments.AddRange(request.EditMapPoint.NewAttachments
            .Select(newAttachment => _attachmentMapper.MapPointAttachment(newAttachment, mapPoint.Id)));

        await _mapPointRepository.UpdateAsync(cancellationToken);

        return Result.Ok();
    }
}
