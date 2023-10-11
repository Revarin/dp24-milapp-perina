using Kris.Client.Data;

namespace Kris.Client
{
    public interface ISessionFacade
    {
        Task<User> CreateUserAsync(string userName);
        Task UpdateUserAsync(int id, string userName);
        Task<bool> UserExistsAsync(int id);
    }
}
