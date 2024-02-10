using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Kris.Server.Common.Options;
using Kris.Server.Data.Models;
using Kris.Server.Common.Models;
using Kris.Common.Enums;
using Kris.Server.Common;

namespace Kris.Server.Core.Services;

public sealed class JwtService : IJwtService
{
    private readonly JwtOptions _jwtOptions;

    public JwtService(IOptions<JwtOptions> jwtOptions)
    {
        _jwtOptions = jwtOptions.Value;
    }

    // Source: https://medium.com/@meghnav274/simple-jwt-authentication-using-asp-net-core-api-5d04b496d27b
    public JwtToken CreateToken(UserEntity user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_jwtOptions.Key);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Login)
            }),
            Issuer = _jwtOptions.Issuer,
            Expires = DateTime.UtcNow.AddMinutes(_jwtOptions.ExpirationMinutes),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return new JwtToken(tokenHandler.WriteToken(token));
    }

    public JwtToken CreateToken(UserEntity user, SessionEntity session, UserType userType)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_jwtOptions.Key);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Login),
                new Claim(KrisClaimTypes.SessionId, session.Id.ToString()),
                new Claim(KrisClaimTypes.SessionName, session.Name),
                new Claim(ClaimTypes.Role, userType.ToString())
            }),
            Expires = DateTime.UtcNow.AddMinutes(_jwtOptions.ExpirationMinutes),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return new JwtToken(tokenHandler.WriteToken(token));
    }
}
