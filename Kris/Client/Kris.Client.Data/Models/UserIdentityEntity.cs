using Kris.Common.Enums;
using Kris.Common.Models;

namespace Kris.Client.Data.Models;

public class UserIdentityEntity
{
    public Guid UserId {  get; set; }
    public string Login { get; set; }
    public Session? CurrentSession { get; set; }

    public class Session
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public UserType UserType { get; set; }
        public string Nickname { get; set; }
        public MapPointSymbol Symbol { get; set; }
    }
}
