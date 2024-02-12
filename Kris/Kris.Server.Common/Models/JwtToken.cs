namespace Kris.Server.Common.Models;

public struct JwtToken
{
    public string Token { get; set; }

    public JwtToken(string token)
    {
        Token = token;
    }
}
