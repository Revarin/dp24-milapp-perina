using AutoMapper;
using FluentResults;
using Kris.Server.Common.Errors;
using Kris.Server.Core.Requests;
using Kris.Server.Data.Models;
using Kris.Server.Data.Repositories;
using MediatR;

namespace Kris.Server.Core.Handlers.User;

public sealed class RegisterUserCommandHandler : UserHandler, IRequestHandler<RegisterUserCommand, Result>
{
    public RegisterUserCommandHandler(IUserRepository userRepository, IMapper mapper) : base(userRepository, mapper)
    {
    }

    public async Task<Result> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var login = request.RegisterUser.Login;

        var userExists = await _userRepository.UserExistsAsync(login, cancellationToken);
        if (userExists) return Result.Fail(new UserExistsError(login));

        var user = _mapper.Map<UserEntity>(request);
        if (user == null) throw new AutoMapperMappingException(nameof(request));

        await _userRepository.InsertAsync(user, cancellationToken);

        return Result.Ok();
    }
}
