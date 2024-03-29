using FluentResults;
using Kris.Interface.Models;
using Kris.Interface.Responses;
using Kris.Server.Common.Errors;
using Kris.Server.Common.Exceptions;
using Kris.Server.Core.Mappers;
using Kris.Server.Core.Requests;
using Kris.Server.Core.Services;
using Kris.Server.Data.Repositories;
using MediatR;

namespace Kris.Server.Core.Handlers.User;

public sealed class LoginUserCommandHandler : UserHandler, IRequestHandler<LoginUserCommand, Result<LoginResponse>>
{
    private readonly IPasswordService _passwordService;
    private readonly IJwtService _jwtService;

    public LoginUserCommandHandler(IPasswordService passwordService, IJwtService jwtService, IUserRepository userRepository, IUserMapper mapper)
        : base(userRepository, mapper)
    {
        _passwordService = passwordService;
        _jwtService = jwtService;
    }

    public async Task<Result<LoginResponse>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByLoginAsync(request.LoginUser.Login, cancellationToken);
        if (user == null) return Result.Fail(new InvalidCredentialsError());

        var passwordVerified = _passwordService.VerifyPassword(user.Password, request.LoginUser.Password);
        if (!passwordVerified) return Result.Fail(new InvalidCredentialsError());

        var jwt = _jwtService.CreateToken(_userMapper.Map(user));
        if (string.IsNullOrEmpty(jwt.Token)) throw new JwtException("Failed to create token");

        var response = new LoginResponse
        {
            UserId = user.Id,
            Login = user.Login,
            Token = jwt.Token,
            JoinedSessions = user.AllSessions.Select(s => s.SessionId),
            CurrentSession = user.CurrentSession?.Session == null ? null : new IdentityResponse.Session
            {
                Id = user.CurrentSession.SessionId,
                Name = user.CurrentSession.Session.Name,
                UserType = user.CurrentSession.UserType,
                Nickname = user.CurrentSession.Nickname,
                Symbol = user.CurrentSession.Symbol
            },
            Settings = new LoginResponse.UserSettings
            {
                ConnectionSettings = user.Settings.IsConnectionSettingsNull() ? null : new ConnectionSettingsModel
                {
                    GpsRequestInterval = user.Settings.GpsRequestInterval.GetValueOrDefault(),
                    PositionUploadFrequency = user.Settings.PositionUploadFrequency.GetValueOrDefault(),
                    PositionDownloadFrequency = user.Settings.PositionDownloadFrequency.GetValueOrDefault(),
                    MapObjectDownloadFrequency = user.Settings.MapObjectDownloadFrequency.GetValueOrDefault()
                },
                MapSettings = user.Settings.IsMapSettingsNull() ? null : new MapSettingsModel
                {
                    CoordinateSystem = user.Settings.CoordinateSystem.GetValueOrDefault(),
                    MapType = user.Settings.MapType.GetValueOrDefault()
                }
            }
        };

        return Result.Ok(response);
    }
}
