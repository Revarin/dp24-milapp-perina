using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Kris.Server.Common.Options;
using Kris.Server.Common;
using Kris.Common.Models;
using Kris.Server.Core.Models;

namespace Kris.Server.Core.Services;

public sealed class JwtService : IJwtService
{
    private readonly JwtOptions _jwtOptions;

    public JwtService(IOptions<JwtOptions> jwtOptions)
    {
        _jwtOptions = jwtOptions.Value;
    }

    // Source: https://medium.com/@meghnav274/simple-jwt-authentication-using-asp-net-core-api-5d04b496d27b
    public JwtToken CreateToken(CurrentUserModel user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_jwtOptions.Key);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Name, user.Login),
                new Claim(KrisClaimTypes.SessionId, user.SessionId.GetValueOrDefault().ToString() ?? string.Empty),
                new Claim(KrisClaimTypes.SessionName, user.SessionName ?? string.Empty),
                new Claim(ClaimTypes.Role, user.UserType.GetValueOrDefault().ToString() ?? string.Empty)
            }),
            Issuer = _jwtOptions.Issuer,
            Expires = DateTime.UtcNow.AddMinutes(_jwtOptions.ExpirationMinutes),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return new JwtToken(tokenHandler.WriteToken(token));
    }
}
