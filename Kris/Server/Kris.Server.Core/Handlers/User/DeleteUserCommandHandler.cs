using FluentResults;
using Kris.Server.Common.Errors;
using Kris.Server.Common.Exceptions;
using Kris.Server.Core.Mappers;
using Kris.Server.Core.Requests;
using Kris.Server.Data.Repositories;
using MediatR;

namespace Kris.Server.Core.Handlers.User;

public sealed class DeleteUserCommandHandler : UserHandler, IRequestHandler<DeleteUserCommand, Result>
{
    public DeleteUserCommandHandler(IUserRepository userRepository, IUserMapper mapper)
        : base(userRepository, mapper)
    {
    }

    public async Task<Result> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetWithSessionsAsync(request.User.Id, cancellationToken);
        if (user == null) throw new NullableException();
        if (user.Login != request.User.Login || user.CurrentSessionId != request.User.SessionId)
            return Result.Fail(new UnauthorizedError("Invalid user data in token"));

        var deleted = await _userRepository.DeleteAsync(request.User.Id, cancellationToken);
        if (!deleted) throw new DatabaseException("Failed to delete user");

        return Result.Ok();
    }
}
