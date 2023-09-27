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
    }
}
