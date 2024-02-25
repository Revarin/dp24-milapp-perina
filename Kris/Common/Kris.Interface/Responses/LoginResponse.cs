using Kris.Common.Enums;

namespace Kris.Interface.Responses;

public sealed class LoginResponse : Response
{
    public Guid UserId { get; set; }
    public string Login { get; set; }
    public string Token { get; set; }
    public Session? CurrentSession { get; set; }
    public IEnumerable<Guid> JoinedSessions { get; set; }

    public sealed class Session
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public UserType UserType { get; set; }
    }
}
