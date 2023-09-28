using Kris.Interface;

namespace Kris.Server
{
    public interface ILocationService : IServiceBase
    {
        bool SaveUserLocation(int userId, DateTime timeStamp, UserLocation location);
        IEnumerable<UserLocation> LoadUsersLocationLocations(int userId, DateTime? timeStamp);
    }
}
