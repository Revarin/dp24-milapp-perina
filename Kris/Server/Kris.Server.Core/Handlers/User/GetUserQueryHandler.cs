using FluentResults;
using Kris.Server.Common.Errors;
using Kris.Server.Core.Mappers;
using Kris.Server.Core.Models;
using Kris.Server.Core.Requests;
using Kris.Server.Data.Repositories;
using MediatR;

namespace Kris.Server.Core.Handlers.User;

public sealed class GetUserQueryHandler : UserHandler, IRequestHandler<GetUserQuery, Result<CurrentUserModel>>
{
    public GetUserQueryHandler(IUserRepository userRepository, IUserMapper mapper) : base(userRepository, mapper)
    {
    }

    public async Task<Result<CurrentUserModel>> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        var entity = await _userRepository.GetAsync(request.Id, cancellationToken);
        if (entity == null) return Result.Fail(new EntityNotFoundError("User", request.Id));

        var user = _userMapper.Map(entity);
        return Result.Ok(user);
    }
}
