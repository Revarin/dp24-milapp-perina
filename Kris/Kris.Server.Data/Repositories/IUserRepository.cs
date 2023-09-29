﻿namespace Kris.Server.Data
{
    public interface IUserRepository : IRepository<UserEntity>
    {
        bool UserExists(string name);
        bool UserExists(int id);
    }
}