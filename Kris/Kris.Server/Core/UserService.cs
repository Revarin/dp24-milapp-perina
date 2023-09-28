using Kris.Server.Data;

namespace Kris.Server
{
    public class UserService : ServiceBase, IUserService
    {
        private readonly IUserRepository _userRepo;

        public UserService(IUserRepository userRepo)
        {
            _userRepo = userRepo;
        }

        public UserEntity CreateUser(string name)
        {
            if (_userRepo.UserExists(name)) return SetErrorMessage<UserEntity>("Username already exists");

            var newUser = _userRepo.Insert(new UserEntity
            {
                Name = name,
                CreatedDate = DateTime.UtcNow,
            });
            return newUser;
        }

        public bool UpdateUserName(int id, string name)
        {
            var user = _userRepo.Get(id);

            if (user == null) return SetErrorMessage("User not found", false);
            if (_userRepo.UserExists(name)) return SetErrorMessage("Username already exists", false);

            user.Name = name;
            _userRepo.Update(user);

            return true;
        }
    }
}
