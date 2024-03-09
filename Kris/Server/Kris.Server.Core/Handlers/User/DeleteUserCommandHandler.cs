using FluentResults;
using Kris.Server.Common.Errors;
using Kris.Server.Common.Exceptions;
using Kris.Server.Core.Mappers;
using Kris.Server.Core.Requests;
using Kris.Server.Core.Services;
using Kris.Server.Data.Repositories;
using MediatR;

namespace Kris.Server.Core.Handlers.User;

public sealed class DeleteUserCommandHandler : UserHandler, IRequestHandler<DeleteUserCommand, Result>
{
    private readonly IPasswordService _passwordService;

    public DeleteUserCommandHandler(IPasswordService passwordService,
        IUserRepository userRepository, IUserMapper mapper)
        : base(userRepository, mapper)
    {
        _passwordService = passwordService;
    }

    public async Task<Result> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetWithSessionsAsync(request.User.UserId, cancellationToken);
        if (user == null) throw new NullableException();
        if (user.Login != request.User.Login) return Result.Fail(new UnauthorizedError("Invalid token"));

        var passwordVerified = _passwordService.VerifyPassword(user.Password, request.Password);
        if (!passwordVerified) return Result.Fail(new InvalidCredentialsError());

        _userRepository.Delete(user);
        await _userRepository.UpdateAsync(cancellationToken);

        return Result.Ok();
    }
}
