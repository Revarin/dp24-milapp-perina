namespace Kris.Server.Data
{
    public class UserRepository : RepositoryBase<UserEntity>, IUserRepository
    {
        public UserRepository(DataContext dataContext) : base(dataContext)
        {
        }

        public bool UserExists(string name)
        {
            return _context.Users.Any(x => x.Name == name);
        }

        public bool UserExists(int id)
        {
            return _context.Users.Any(x => x.Id == id);
        }
    }
}
