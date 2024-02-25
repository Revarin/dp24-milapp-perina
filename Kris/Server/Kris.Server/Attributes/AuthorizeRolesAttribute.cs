using Kris.Common.Enums;
using Microsoft.AspNetCore.Authorization;

namespace Kris.Server.Attributes;

// Source: https://stackoverflow.com/a/24182340
public sealed class AuthorizeRolesAttribute : AuthorizeAttribute
{
    public AuthorizeRolesAttribute(params UserType[] roles) : base()
    {
        Roles = string.Join(",", roles);
    }
}
