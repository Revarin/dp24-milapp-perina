using Kris.Client.Data;
using Kris.Interface;

namespace Kris.Client
{
    public class SessionFacade : ISessionFacade
    {
        private readonly ISessionController _sessionClient;

        public SessionFacade(ISessionController sessionClient)
        {
            _sessionClient = sessionClient;
        }

        public async Task<User> CreateUserAsync(string userName)
        {
            var response = await _sessionClient.CreateUser(new CreateUserRequest
            {
                Name = userName,
            });

            return new User
            {
                Id = response.Id,
                Name = response.Name
            };
        }

        public async Task UpdateUserAsync(int id, string userName)
        {
            await _sessionClient.UpdateUserName(new UpdateUserNameRequest
            {
                Id = id,
                Name = userName
            });
        }
    }
}
