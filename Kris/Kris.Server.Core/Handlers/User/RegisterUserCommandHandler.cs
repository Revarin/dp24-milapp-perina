using FluentResults;
using MediatR;
using Kris.Server.Common.Errors;
using Kris.Server.Core.Mappers;
using Kris.Server.Core.Requests;
using Kris.Server.Data.Repositories;

namespace Kris.Server.Core.Handlers.User;

public sealed class RegisterUserCommandHandler : UserHandler, IRequestHandler<RegisterUserCommand, Result>
{
    public RegisterUserCommandHandler(IUserRepository userRepository, IUserMapper mapper)
        : base(userRepository, mapper)
    {
    }

    public async Task<Result> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var login = request.RegisterUser.Login;

        var userExists = await _userRepository.UserExistsAsync(login, cancellationToken);
        if (userExists) return Result.Fail(new EntityExistsError("User", login));

        var user = _userMapper.Map(request);

        await _userRepository.InsertAsync(user, cancellationToken);

        return Result.Ok();
    }
}
